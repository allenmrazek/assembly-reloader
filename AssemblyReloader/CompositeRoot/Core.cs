using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using AssemblyReloader.Annotations;
using AssemblyReloader.Commands;
using AssemblyReloader.Controllers;
using AssemblyReloader.DataObjects;
using AssemblyReloader.Destruction;
using AssemblyReloader.Game;
using AssemblyReloader.Game.Providers;
using AssemblyReloader.Game.Queries;
using AssemblyReloader.Generators;
using AssemblyReloader.Gui;
using AssemblyReloader.Loaders;
using AssemblyReloader.Loaders.PartModuleLoader;
using AssemblyReloader.Providers;
using AssemblyReloader.Queries;
using AssemblyReloader.Queries.AssemblyQueries;
using AssemblyReloader.Queries.CecilQueries;
using AssemblyReloader.Queries.FileSystemQueries;
using AssemblyReloader.Repositories;
using AssemblyReloader.TypeInstallers;
using AssemblyReloader.Weaving;
using AssemblyReloader.Weaving.Operations;
using Contracts;
using Mono.Cecil;
using Mono.Cecil.Cil;
using ReeperCommon.FileSystem;
using ReeperCommon.FileSystem.Factories;
using ReeperCommon.FileSystem.Implementations;
using ReeperCommon.FileSystem.Implementations.Providers;
using ReeperCommon.Gui.Window.Providers;
using ReeperCommon.Logging;
using ReeperCommon.Logging.Implementations;
using ReeperCommon.Repositories.Resources;
using ReeperCommon.Repositories.Resources.Implementations;
using ReeperCommon.Repositories.Resources.Implementations.Decorators;
using UnityEngine;

namespace AssemblyReloader.CompositeRoot
{
    // composite root
    class Core
    {
        private readonly ILog _log;
        private readonly IMessageChannel _messageChannel;



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
            var eventProvider = ConfigureEventProvider(new StartupSceneFromGameSceneQuery());

            KspPartActionWindowListener.WindowController = new KspPartActionWindowController();
            KspPartActionWindowListener.PartActionWindowQuery =
                new ComponentsInGameObjectHierarchyProvider<UIPartActionWindow>();

            eventProvider.OnSceneLoaded.OnEvent += s =>
            {
                if (UIPartActionController.Instance != null && UIPartActionController.Instance.windowPrefab != null)
                    if (UIPartActionController.Instance.windowPrefab.GetComponent<KspPartActionWindowListener>() == null)
                        UIPartActionController.Instance.windowPrefab.gameObject.AddComponent<KspPartActionWindowListener>();
            };

            var loadedAssemblyFactory = ConfigureLoadedAssemblyFactory();

            var reloadables = CreateReloadablePlugins(loadedAssemblyFactory, fsFactory, assemblyResolver).ToList();

            reloadables.ForEach(r => r.Load());

            var skinScheme = ConfigureSkin();

            var windowFactory = new WindowFactory(resourceLocator, skinScheme);
            var uniqueIdProvider = new UniqueWindowIdProvider();
            var expandablePanelFactory = windowFactory.CreateExpandablePanelFactory(skinScheme);

            var guiController =
                new GuiController(reloadables.ToDictionary(r => r,
                    r =>
                    {
                        var configWindow = windowFactory.CreatePluginOptionsWindow(r, expandablePanelFactory);

                        configWindow.Visible = false;

                        return
                            new ReloadablePluginController(r, configWindow) as IReloadablePluginController;
                    }));

            windowFactory.CreateMainWindow(
                new View(guiController),
                new Rect(400f, 400f, 250f, 128f),
                uniqueIdProvider.Get());

    


            //AssemblyLoader.loadedAssemblies.ToList().ForEach(la =>
            //{
            //    _log.Normal("LoadedAssembly: " + la.dllName);
            //    _log.Normal("  Types:");
            //    la.types.ToList().ForEach(ty =>
            //    {
            //        _log.Normal("    BaseType: " + ty.Key);
            //        _log.Normal("       Contains: ");
            //        ty.Value.ForEach(v => _log.Normal("        Type: " + v.FullName + "; " + v.AssemblyQualifiedName));
            //    });
            //});

        }


        [UsedImplicitly]
        public void Tick()
        {
            
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
                    new ResourceFromDirectory(dllDirectory, 1),


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


        private ILoadedAssemblyFactory ConfigureLoadedAssemblyFactory()
        {
            return new KspLoadedAssemblyFactory(
                new DisposeLoadedAssemblyCommandFactory(),
                new GenericTypeInstaller<Part>(new TypesDerivedFromQuery<Part>()),
                new GenericTypeInstaller<PartModule>(new TypesDerivedFromQuery<PartModule>()),
                new GenericTypeInstaller<ScenarioModule>(new TypesDerivedFromQuery<ScenarioModule>()));
        }


        private IEnumerable<IReloadablePlugin> CreateReloadablePlugins(
            ILoadedAssemblyFactory laFactory,
            IFileSystemFactory fsFactory, 
            BaseAssemblyResolver assemblyResolver)
        {
            var reloadableAssemblyFileQuery = new ReloadableAssemblyFilesInDirectoryQuery(fsFactory.GetGameDataDirectory());

            var addonDestroyer = new AddonDestroyer(
                new UnityObjectDestroyer(new PluginReloadRequestedMethodCallCommand()),
                new LoadedComponentQuery(),
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

            var debugSymbolExistQuery = new DebugSymbolFileExistsQuery(location);

            var assemblyProvider = new AssemblyProvider(
                new AssemblyDefinitionFromDiskReader(
                    location, 
                    debugSymbolExistQuery,
                    assemblyResolver),
                new AssemblyDefinitionLoader(
                    new TemporaryFileFactory(
                        location.Directory,
                        new RandomStringGenerator()),
                    _log.CreateTag("AssemblyDefinitionLoader")),
                ConfigureDefinitionWeaver(location));


            

            var kspLoader = new KspAssemblyLoader(assemblyProvider, laFactory);

            var reloadable = new ReloadablePlugin(kspLoader, location, ConfigurePluginConfiguration());

            var kspFactory = new KspFactory(new KspGameObjectProvider());

            var descriptorFactory = new PartModuleDescriptorFactory(
                                        new KspPartLoader(
                                            kspFactory),
                                        new AvailablePartConfigQuery(
                                            new KspGameDatabase()),
                                        new ModuleConfigsFromPartConfigQuery(),
                                        new TypeIdentifierQuery());

            var prefabCloneProvider = new PartPrefabCloneProvider(
                new LoadedComponentQuery<Part>(),
                new ComponentsInGameObjectHierarchyProvider<Part>(),
                new PartIsPrefabQuery(),
                kspFactory);

            var partModuleRepository = new FlightConfigRepository();

            var protoScenarioModuleProvider = new ProtoScenarioModuleProvider(
                kspFactory,
                new TypeIdentifierQuery(),
                new CurrentGameProvider(new TypeIdentifierQuery()));

            var gameProvider = new CurrentGameProvider(new TypeIdentifierQuery());
            
            var scenarioModuleController =
                new ScenarioModuleController(
                    new ScenarioModuleLoader(protoScenarioModuleProvider),
                    new ScenarioModuleUnloader(
                        gameProvider,
                        new GameObjectComponentQuery(new KspGameObjectProvider()),
                        protoScenarioModuleProvider,
                        new UnityObjectDestroyer(new PluginReloadRequestedMethodCallCommand()),
                        true,
                        _log.CreateTag("ScenarioModuleUnloader")),
                    new TypesDerivedFromQuery<ScenarioModule>(),
                    new CurrentGameSceneProvider());

            reloadable.OnLoaded += scenarioModuleController.Load;
            reloadable.OnUnloaded += scenarioModuleController.Unload;


            var partModuleController = new PartModuleController(
                new PartModuleLoader(
                    descriptorFactory,
                    new PartModuleFactory(new PartIsPrefabQuery(), new AwakenPartModuleCommand()),
                    partModuleRepository,
                    prefabCloneProvider),
                new PartModuleUnloader(
                    new UnityObjectDestroyer(new PluginReloadRequestedMethodCallCommand()),
                    descriptorFactory,
                    prefabCloneProvider,
                    ConfigurePartModuleSnapshotGenerator(partModuleRepository)
                    ),
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


 


        private GUISkin ConfigureSkin()
        {
            //Resources.FindObjectsOfTypeAll<GUISkin>().ToList().ForEach(s => new DebugLog().Debug("Skin: " + s.name));

            //UnityEngine.Object.FindObjectOfType<AssetBase>()
            //    .guiSkins.ToList()
            //    .ForEach(g => new DebugLog().Debug("AB: Skin: " + g.name));

            //var skin = UnityEngine.Object.Instantiate(HighLogic.Skin) as GUISkin;
            //var skin = UnityEngine.Object.Instantiate(AssetBase.GetGUISkin("KSP window 5")) as GUISkin;
            //var skin = UnityEngine.Object.Instantiate(AssetBase.GetGUISkin("KSP window 4")) as GUISkin;
            //var skin = UnityEngine.Object.Instantiate(AssetBase.GetGUISkin("KSP window 4")) as GUISkin;
            //var skin = UnityEngine.Object.Instantiate(AssetBase.GetGUISkin("KSP window 3")) as GUISkin;
            //var skin = UnityEngine.Object.Instantiate(AssetBase.GetGUISkin("KSP window 1")) as GUISkin;
            var skin = UnityEngine.Object.Instantiate(AssetBase.GetGUISkin("OrbitMapSkin")) as GUISkin;


            skin.window.padding.left = skin.window.padding.right = 3;

            return skin;
        }


        private IConfiguration ConfigurePluginConfiguration( /* context -- filename? */)
        {
            return new Configuration(new ConfigNode()); // todo: load actual ConfigNode settings here
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
   
            var renameAssembly = new RenameAssemblyOperation(new UniqueAssemblyNameGenerator(new RandomStringGenerator()));

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

            return new AssemblyDefinitionWeaver(
                _log.CreateTag("Weaver"), 
                allTypesFromAssemblyExceptInjected,
                new AllMethodsFromDefinitionQuery(),
                renameAssembly,
                writeInjectedHelper,
                replaceAssemblyLocationCalls);

        }


        private IPartModuleSnapshotGenerator ConfigurePartModuleSnapshotGenerator(IFlightConfigRepository repository)
        {
            if (repository == null) throw new ArgumentNullException("repository");

            return new PartModuleSnapshotGenerator(
                repository,
                new PartIsPrefabQuery(),
                new TypeIdentifierQuery(),
                new UniqueFlightIdGenerator(),
                _log.CreateTag("PMSnapshotter"));
        }


        private IExpandablePanelFactory ConfigureExpandablePanelFactory(GUISkin skinScheme)
        {
            var style = new GUIStyle(skinScheme.toggle);

            // todo: make any adjustments to toggle button here

            return new ExpandablePanelFactory(style, 14f, 0f, false);
        }
    }
}
