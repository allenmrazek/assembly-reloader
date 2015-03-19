using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using AssemblyReloader.CompositeRoot.Commands;
using AssemblyReloader.CompositeRoot.MonoBehaviours;
using AssemblyReloader.Controllers;
using AssemblyReloader.Destruction;
using AssemblyReloader.Disk;
using AssemblyReloader.Game;
using AssemblyReloader.Generators;
using AssemblyReloader.GUI;
using AssemblyReloader.Loaders;
using AssemblyReloader.Logging;
using AssemblyReloader.Messages;
using AssemblyReloader.PluginTracking;
using AssemblyReloader.Providers;
using AssemblyReloader.Providers.SceneProviders;
using AssemblyReloader.Queries;
using AssemblyReloader.Queries.AssemblyQueries;
using AssemblyReloader.Queries.CecilQueries;
using AssemblyReloader.Queries.CecilQueries.Injected;
using AssemblyReloader.Queries.CecilQueries.IntermediateLanguage;
using AssemblyReloader.Queries.ConfigNodeQueries;
using AssemblyReloader.Queries.ConversionQueries;
using AssemblyReloader.Queries.FileSystemQueries;
using AssemblyReloader.Repositories;
using AssemblyReloader.Weaving;
using AssemblyReloader.Weaving.Commands;
using AssemblyReloader.Weaving.Operations;
using Contracts;
using Mono.Cecil;
using Mono.Cecil.Cil;
using ReeperCommon.FileSystem;
using ReeperCommon.FileSystem.Factories;
using ReeperCommon.FileSystem.Implementations;
using ReeperCommon.FileSystem.Implementations.Providers;
using ReeperCommon.Gui.Window.View;
using ReeperCommon.Logging;
using ReeperCommon.Logging.Factories;
using ReeperCommon.Logging.Implementations;
using ReeperCommon.Repositories.Resources;
using ReeperCommon.Repositories.Resources.Implementations;
using ReeperCommon.Repositories.Resources.Implementations.Decorators;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AssemblyReloader.CompositeRoot
{
    // composite root
    class Core
    {
        private readonly IEventProvider _eventProvider;

        private readonly WindowView _view;
        private readonly WindowView _logView;

        private readonly IReloadablePluginController _pluginController;
        private readonly MessageChannel _messageChannel;


        private readonly ILog _log;



        private interface IConsumer
        {
            void Consume(object message);
        }


        private class MessageChannel : IMessageChannel
        {
            private readonly List<IConsumer> _consumers;

            public MessageChannel(params IConsumer[] consumers)
            {
                _consumers = new List<IConsumer>(consumers);
            }


            public void Send<T>(T message)
            {
                _consumers.ForEach(consumer => consumer.Consume(message));
            }

            public void AddListener<T>(object listener)
            {
                AddConsumer(new Consumer<T>(listener));
            }

            public void RemoveListener(object listener)
            {
                _consumers.RemoveAll(ic => ReferenceEquals(ic, listener));
            }


            private void AddConsumer(IConsumer consumer)
            {
                if (consumer == null) throw new ArgumentNullException("consumer");

                if (!_consumers.Contains(consumer))
                    _consumers.Add(consumer);
            }


        }





        private class Consumer<T> : IConsumer
        {
            private readonly object _consumer;

            public Consumer(object consumer)
            {
                if (consumer == null) throw new ArgumentNullException("consumer");
                if (!(consumer is IMessageConsumer<T>)) throw new InvalidOperationException("consumer is not a " + typeof (T).Name);

                _consumer = consumer;
            }


            public void Consume(object message)
            {
                if (message is T && _consumer is IMessageConsumer<T>)
                    (_consumer as IMessageConsumer<T>).Consume((T)message);
            }
        }


        private class EventProvider : IEventProvider
        {
            public IGameEventPublisher<GameScenes> OnLevelWasLoaded { get; set; }
            public IGameEventPublisher<KSPAddon.Startup> OnSceneLoaded { get; set; }
        }



       

        public Core()
        {

#if DEBUG
            var primaryLog = new DebugLog("ART");
#else
            var primaryLog = LogFactory.Create(LogLevel.Standard);
#endif

            var cachedLog = new CachedLog(primaryLog, 100);

            _log = cachedLog;


            var fsFactory = new KSPFileSystemFactory(
                new KSPUrlDir(new KSPGameDataUrlDirProvider().Get()));

            var ourDirProvider = new AssemblyDirectoryQuery(Assembly.GetExecutingAssembly(), fsFactory.GetGameDataDirectory());

            var assemblyResolver = new DefaultAssemblyResolver();

            assemblyResolver.AddSearchDirectory(ourDirProvider.Get().FullPath); // we'll be importing some references to types we own so this is a necessary step


            var resourceLocator = ConfigureResourceRepository(ourDirProvider.Get());
            _eventProvider = ConfigureEventProvider(new StartupSceneFromGameSceneQuery());

            KspPartActionWindowListener.WindowController = new KspPartActionWindowController();
            KspPartActionWindowListener.PartActionWindowQuery =
                new ComponentsInGameObjectHierarchyProvider<UIPartActionWindow>();

            _eventProvider.OnSceneLoaded.OnEvent += s =>
            {
                if (UIPartActionController.Instance != null && UIPartActionController.Instance.windowPrefab != null)
                    if (UIPartActionController.Instance.windowPrefab.GetComponent<KspPartActionWindowListener>() == null)
                        UIPartActionController.Instance.windowPrefab.gameObject.AddComponent<KspPartActionWindowListener>();
            };
            

            var reloadables = CreateReloadablePlugins(fsFactory, assemblyResolver).ToList();

            reloadables.ForEach(r => r.Load());

            _pluginController = new ReloadablePluginController(reloadables, _log.CreateTag("PluginController"));

            var windowFactory = new WindowFactory(resourceLocator, cachedLog);

            var logWindow = windowFactory.CreateLogWindow();
            var mainWindow = windowFactory.CreateMainWindow(new MainViewWindowLogic(_pluginController, logWindow));

            _view = WindowView.Create(mainWindow);
            _logView = WindowView.Create(logWindow);

            Object.DontDestroyOnLoad(_view);
            Object.DontDestroyOnLoad(_logView);

            AssemblyLoader.loadedAssemblies.ToList().ForEach(la =>
            {
                _log.Normal("LoadedAssembly: " + la.dllName);
                _log.Normal("  Types:");
                la.types.ToList().ForEach(ty =>
                {
                    _log.Normal("    BaseType: " + ty.Key);
                    _log.Normal("       Contains: ");
                    ty.Value.ForEach(v => _log.Normal("        Type: " + v.FullName));
                });
            });
        }







        private IResourceRepository ConfigureResourceRepository(IDirectory dllDirectory)
        {
            return new ResourceRepositoryComposite(
                // search GameDatabase first
                //   note: GameDatabase expects extensionless urls
                    new ResourceIdentifierAdapter(id =>
                    {
                        if (!Path.HasExtension(id) || string.IsNullOrEmpty(id)) return id;

                        var dir = Path.GetDirectoryName(id) ?? "";
                        var woExt = Path.Combine(dir, Path.GetFileNameWithoutExtension(id)).Replace('\\', '/');

                        return !string.IsNullOrEmpty(woExt) ? woExt : id;
                    }, new ResourceFromGameDatabase()
                    ),

                // then look at physical file system. These work on a list of items cached
                // by GameDatabase rather than working directly with the disk (unless a resource 
                // is accessed from here, of course)
                    new ResourceFromDirectory(dllDirectory),


                // finally search embedded resource
                //   note: embedded resource ids should be appended by assembly namespace
                    new ResourceIdentifierAdapter(id => Assembly.GetExecutingAssembly().GetName().Name + "." + id,

                //   note: expects resource manifest ids (periods instead of slashes), so 
                //         identifier transformer is needed if we want to specify input identifiers
                //         as though they were paths
                    new ResourceIdentifierAdapter(id => id.Replace('/', '.').Replace('\\', '.'),
                        new ResourceFromEmbeddedResource(Assembly.GetExecutingAssembly()))
                    ));
        }



        private IEnumerable<IReloadablePlugin> CreateReloadablePlugins(
            IFileSystemFactory fsFactory, 
            BaseAssemblyResolver assemblyResolver)
        {
            var laFactory = new KspLoadedAssemblyFactory(
                new TypesDerivedFromQuery<Part>(),
                new TypesDerivedFromQuery<PartModule>(),
                new TypesDerivedFromQuery<InternalModule>(),
                new TypesDerivedFromQuery<ScenarioModule>(),
                new TypesDerivedFromQuery<Contract>());


            var reloadableAssemblyFileQuery = new ReloadableAssemblyFilesInDirectoryQuery(fsFactory.GetGameDataDirectory());

            var addonDestroyer = new AddonDestroyer(
                new UnityObjectDestroyer(new PluginReloadRequestedMethodCallCommand()),
                new LoadedComponentProvider(),
                new AddonsFromAssemblyQuery(new AddonAttributesFromTypeQuery()));

            return reloadableAssemblyFileQuery
                .Get()
                .Select(raFile => ConfigureReloadablePlugin(raFile, 
                    assemblyResolver, 
                    laFactory, 
                    addonDestroyer));
        }


        private IEventProvider ConfigureEventProvider(IStartupSceneFromGameSceneQuery query)
        {
            if (query == null) throw new ArgumentNullException("query");

            var onLevelWasLoaded = new GameEventPublisher<GameScenes>();
            GameEvents.onLevelWasLoaded.Add(onLevelWasLoaded.Raise);

            var onSceneLoaded = new GameEventPublisher<KSPAddon.Startup>();
            onLevelWasLoaded.OnEvent += gameScene => onSceneLoaded.Raise(query.Get(gameScene));

            return new EventProvider{OnLevelWasLoaded = onLevelWasLoaded,
                OnSceneLoaded = onSceneLoaded};
        }



        private IReloadablePlugin ConfigureReloadablePlugin(
            IFile location, 
            BaseAssemblyResolver assemblyResolver, 
            ILoadedAssemblyFactory laFactory,
            IAddonDestroyer addonDestroyer)
        {

            var assemblyProvider = new AssemblyFromDefinitionProvider();
            var definitionReader = new AssemblyDefinitionReader(location, assemblyResolver);

            var kspLoader = new KspAssemblyLoader(definitionReader,
                assemblyProvider,
                laFactory,
                ConfigureDefinitionWeaver(location),
#if DEBUG
                true
#else
                false
#endif
                );

            var reloadable = new ReloadablePlugin(kspLoader, location);

            
            
            var kspFactory = new KspFactory();

            var descriptorFactory = new PartModuleDescriptorFactory(
                                        new KspPartLoader(
                                            kspFactory),
                                        new AvailablePartConfigProvider(
                                            new KspGameDatabase()),
                                        new ModuleConfigsFromPartConfigQuery(),
                                        new TypeIdentifierQuery());

            var prefabCloneProvider = new PartPrefabCloneProvider(
                new LoadedComponentProvider<Part>(),
                new ComponentsInGameObjectHierarchyProvider<Part>(),
                new PartIsPrefabQuery(),
                kspFactory);

            var partModuleRepository = new FlightConfigRepository();

            var partModuleController = new PartModuleController(
                new PartModuleLoader(
                    descriptorFactory,
                    new PartModuleFactory(new PartIsPrefabQuery()),
                    partModuleRepository,
                    prefabCloneProvider),
                new PartModuleUnloader(
                    ConfigurePartModuleDestroyer(partModuleRepository),
                    descriptorFactory,
                    prefabCloneProvider),
                new TypesDerivedFromQuery<PartModule>(),
                partModuleRepository,
                new RefreshPartActionWindows(KspPartActionWindowListener.WindowController),
                _log.CreateTag("PartModuleController"));

            reloadable.OnLoaded += partModuleController.Load;
            reloadable.OnUnloaded += partModuleController.Unload;




            var addonController =
                new AddonController(
                    new KspAddonLoader(),
                    addonDestroyer,
                    new CurrentStartupSceneProvider(new StartupSceneFromGameSceneQuery(),
                    new CurrentGameSceneProvider()));

            reloadable.OnLoaded += addonController.Load;
            reloadable.OnUnloaded += addonController.Unload;



            return reloadable;
        }


        private IPartModuleDestroyer ConfigurePartModuleDestroyer(IFlightConfigRepository configRepository)
        {
            
            var idGenerator = new UniqueFlightIdGenerator();
            var identifierQuery = new TypeIdentifierQuery();
            var isPrefabQuery = new PartIsPrefabQuery();
            var kspFactory = new KspFactory();

            var destroyer = new PartModuleDestroyer(
                new UnityObjectDestroyer(new PluginReloadRequestedMethodCallCommand()),
                isPrefabQuery,
                kspFactory,
                idGenerator,
                configRepository,
                identifierQuery,
                _log.CreateTag("PartModuleDestroyer"));

            return destroyer;
        }


        private IAssemblyDefinitionWeaver ConfigureDefinitionWeaver(IFile location)
        {
            if (location == null) throw new ArgumentNullException("location");
            
            var getCodeBaseProperty = typeof (Assembly).GetProperty("CodeBase",
                BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty);

            var getLocationProperty = typeof (Assembly).GetProperty("Location",
                BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty);
   
            if (getCodeBaseProperty == null || getCodeBaseProperty.GetGetMethod() == null)
                throw new MissingMethodException(typeof (Assembly).FullName, "CodeBase");

            if (getLocationProperty == null || getCodeBaseProperty.GetGetMethod() == null)
                throw new MissingMethodException(typeof (Assembly).FullName, "Location");


            var uri = new Uri(location.FullPath);
            var allTypesFromAssemblyExceptInjected = new ExcludingTypeDefinitions(
                new AllTypesFromDefinitionQuery(), new InjectedHelperTypeQuery());
   
            var renameAssembly = new RenameAssemblyOperation(new UniqueAssemblyNameGenerator());

            var writeInjectedHelper = new InjectedHelperTypeDefinitionWriter(
                _log.CreateTag("InjectedHelperWriter"),
                new CompositeCommand<TypeDefinition>(
                    new ProxyAssemblyMethodWriter(Uri.UnescapeDataString(uri.AbsoluteUri), getCodeBaseProperty.GetGetMethod()),
                    new ProxyAssemblyMethodWriter(uri.LocalPath, getLocationProperty.GetGetMethod())));

            var replaceAssemblyLocationCalls = new InterceptExecutingAssemblyLocationQueries(
                new MethodCallInMethodBodyQuery(
                    getCodeBaseProperty.GetGetMethod(),
                    OpCodes.Callvirt),
                    new InjectedHelperTypeMethodQuery(new InjectedHelperTypeQuery(), getCodeBaseProperty.GetGetMethod().Name)
                );

            //var insertIntermediateLanguageCode = new InsertIntermediateLanguageCommandsIntoMethod(
            //    _log.CreateTag("InsertIL"),
            //    new CompositeTypeDefinitionQuery(
            //        new TypeDefinitionsDerivedFromBaseTypeQuery<MonoBehaviour>(allTypesFromAssemblyExceptInjected),
            //        new TypeDefinitionsDerivedFromBaseTypeQuery<PartModule>(allTypesFromAssemblyExceptInjected),
            //        new TypeDefinitionsDerivedFromBaseTypeQuery<ScenarioModule>(allTypesFromAssemblyExceptInjected)),
            //    new AllMethodsFromDefinitionQuery());

            return new AssemblyDefinitionWeaver(
                _log.CreateTag("Weaver"), 
                allTypesFromAssemblyExceptInjected,
                new AllMethodsFromDefinitionQuery(),
                renameAssembly,
                writeInjectedHelper,
                replaceAssemblyLocationCalls);

        }
    }
}
