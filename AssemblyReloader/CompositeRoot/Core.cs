using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using AssemblyReloader.CompositeRoot.Commands;
using AssemblyReloader.CompositeRoot.Commands.ILModifications;
using AssemblyReloader.Controllers;
using AssemblyReloader.Destruction;
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
using AssemblyReloader.Queries.ConfigNodeQueries;
using AssemblyReloader.Queries.ConversionQueries;
using AssemblyReloader.Queries.FileSystemQueries;
using AssemblyReloader.Repositories;
using Contracts;
using Mono.Cecil;
using ReeperCommon.FileSystem;
using ReeperCommon.FileSystem.Factories;
using ReeperCommon.FileSystem.Implementations;
using ReeperCommon.FileSystem.Implementations.Providers;
using ReeperCommon.Gui.Window.View;
using ReeperCommon.Logging;
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

            assemblyResolver.AddSearchDirectory(Assembly.GetExecutingAssembly().Location); // we'll be importing some references to types we own so this is a necessary step


            var resourceLocator = ConfigureResourceRepository(ourDirProvider.Get());
            _eventProvider = ConfigureEventProvider(new StartupSceneFromGameSceneQuery());


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
            var ilModifications = ConfigureAssemblyModifications();


            var addonDestroyer = new AddonDestroyer(
                ConfigureDestructionController(new FlightConfigRepository()), // config repo unused
                new LoadedComponentProvider(),
                new AddonsFromAssemblyQuery(new AddonAttributesFromTypeQuery()));

            return reloadableAssemblyFileQuery.Get().Select(raFile => ConfigureReloadablePlugin(raFile, assemblyResolver, laFactory, addonDestroyer, ilModifications));
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


        private IAssemblyProvider ConfigureAssemblyProvider(IFile location, BaseAssemblyResolver resolver,
            ICommand<AssemblyDefinition> ilModifications)
        {
            if (location == null) throw new ArgumentNullException("location");
            if (resolver == null) throw new ArgumentNullException("resolver");
            if (ilModifications == null) throw new ArgumentNullException("ilModifications");

            return new AssemblyFromDefinitionProvider(
                new AssemblyDefinitionReader(location, resolver),
                new AssemblyDefinitionLoader(),
                ilModifications);

        }


        private ICommand<AssemblyDefinition> ConfigureAssemblyModifications()
        {
            return new CompositeCommand<AssemblyDefinition>(
                new RenameAssemblyCommand(new UniqueAssemblyNameGenerator())
                );
        }


        private IReloadablePlugin ConfigureReloadablePlugin(
            IFile location, 
            BaseAssemblyResolver assemblyResolver, 
            ILoadedAssemblyFactory laFactory,
            IAddonDestroyer addonDestroyer,
            ICommand<AssemblyDefinition> ilModifications)
        {
            var reloadable = new ReloadablePlugin(
                    ConfigureAssemblyProvider(location, assemblyResolver, ilModifications), location);

            var kspLoader = new KspAssemblyLoader(laFactory);
            
            var kspFactory = new KspFactory();

            var descriptorFactory = new PartModuleDescriptorFactory(
                                        new KspPartLoader(
                                            kspFactory),
                                        new AvailablePartConfigProvider(
                                            new KspGameDatabase()),
                                        new ModuleConfigsFromPartConfigQuery(),
                                        new TypeIdentifierQuery(),
                                        _log.CreateTag("KspPartLoader"));

            var prefabCloneProvider = new PartPrefabCloneProvider(
                new LoadedComponentProvider<Part>(),
                new ComponentsInGameObjectHierarchyProvider<Part>(),
                new PartIsPrefabQuery(),
                kspFactory);

            var partModuleRepository = new FlightConfigRepository();


            var destructionController = ConfigureDestructionController(partModuleRepository);

            reloadable.OnLoaded += kspLoader.Load;
            reloadable.OnUnloaded += kspLoader.Unload;



            var partModuleController = new PartModuleController(
                new PartModuleLoader(
                    descriptorFactory,
                    new PartModuleFactory(),
                    partModuleRepository,
                    prefabCloneProvider),
                new PartModuleUnloader(
                    destructionController,
                    descriptorFactory,
                    prefabCloneProvider),
                new TypesDerivedFromQuery<PartModule>(),
                partModuleRepository,
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


        private IDestructionController ConfigureDestructionController(IFlightConfigRepository configRepository)
        {
            var controller = new DestructionController();
            var destroyer = new ObjectDestroyer(_log.CreateTag("ObjectDestroyer"));
            var idGenerator = new UniqueFlightIdGenerator();
            var identifierQuery = new TypeIdentifierQuery();

            // todo: experience traits?

            controller.Register<MonoBehaviour>(destroyer.Destroy);

            controller.Register<PartModule>(pm =>
            {
                _log.Normal("Destroying " + pm.GetType().FullName + " on " + pm.part.flightID);

                if (pm.part.flightID == 0)
                    pm.part.flightID = idGenerator.Get();

                _log.Normal("FlightID is now " + pm.part.flightID);

                var cfg = new ConfigNode();

                pm.Save(cfg);

                _log.Normal("Saving ConfigNode: {0}", cfg.ToString());

                configRepository.Store(pm.part.flightID, identifierQuery.Get(pm.GetType()), cfg);

                destroyer.Destroy(pm);
            });

            controller.Register<InternalModule>(im =>
            {
                destroyer.Destroy(im);
                throw new NotImplementedException();
            });

            controller.Register<ScenarioModule>(sm =>
            {
                destroyer.Destroy(sm);
                throw new NotImplementedException();
            });

            controller.Register<Contract>(contract => { throw new NotImplementedException(); });


            return controller;
        }
    }
}
