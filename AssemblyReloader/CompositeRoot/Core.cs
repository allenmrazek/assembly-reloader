using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using AssemblyReloader.CompositeRoot.Commands;
using AssemblyReloader.CompositeRoot.Commands.ILModifications;
using AssemblyReloader.Controllers;
using AssemblyReloader.DataObjects;
using AssemblyReloader.Destruction;
using AssemblyReloader.Game;
using AssemblyReloader.Generators;
using AssemblyReloader.GUI;
using AssemblyReloader.Loaders;
using AssemblyReloader.Loaders.AddonLoader;
using AssemblyReloader.Loaders.PMLoader;
using AssemblyReloader.Logging;
using AssemblyReloader.Messages;
using AssemblyReloader.PluginTracking;
using AssemblyReloader.Providers;
using AssemblyReloader.Providers.SceneProviders;
using AssemblyReloader.Queries;
using AssemblyReloader.Queries.AssemblyQueries;
using AssemblyReloader.Queries.CecilQueries;
using AssemblyReloader.Queries.ConfigNodeQueries;
using AssemblyReloader.Queries.ConversionQueries;
using AssemblyReloader.Queries.FileSystemQueries;
using AssemblyReloader.Repositories;
using Mono.Cecil;
using ReeperCommon.Events.Implementations;
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
using Object = UnityEngine.Object;

namespace AssemblyReloader.CompositeRoot
{
    // composite root
    class Core
    {
        private readonly IEventProvider _eventProvider;

        private readonly WindowView _view;
        private readonly WindowView _logView;

        private readonly IReloadableController _controller;
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

            var ourDirProvider = new AssemblyDirectoryQuery(Assembly.GetExecutingAssembly(), fsFactory.GetGameDataDirectory(), _log);

            var partModuleProxyAssemblyFile = ourDirProvider.Get().File(new KSPUrlIdentifier("PartModule.proxy"));

            var assemblyResolver = new DefaultAssemblyResolver();

            assemblyResolver.AddSearchDirectory(Assembly.GetExecutingAssembly().Location); // we'll be importing some references to types we own so this is a necessary step

            var partModuleProxyAssemblyProvider = new ProxyAssemblyProvider(
                new AssemblyDefinitionReader(partModuleProxyAssemblyFile.Single(), assemblyResolver),
                new AssemblyDefinitionIntoKspLoader(partModuleProxyAssemblyFile.Single(), new KspAssemblyLoader(_log.CreateTag("KspAssemblyLoader"))),
                new PartModuleDefinitionsQuery(),

                new CompositeCommand<AssemblyDefinition>(
                    new RenameAssemblyCommand(new UniqueAssemblyNameGenerator())
                    ));

            var resourceLocator = ConfigureResourceRepository(ourDirProvider.Get());
            _eventProvider = ConfigureEventProvider(new StartupSceneFromGameSceneQuery());


            var reloadables = CreateReloadablePlugins(cachedLog, fsFactory, partModuleProxyAssemblyProvider, assemblyResolver).ToList();

            reloadables.ForEach(r => r.Load());

            _controller = new ReloadableController(reloadables);

            var windowFactory = new WindowFactory(resourceLocator, cachedLog);

            var logWindow = windowFactory.CreateLogWindow();
            var mainWindow = windowFactory.CreateMainWindow(new MainViewWindowLogic(_controller, logWindow));

            _view = WindowView.Create(mainWindow);
            _logView = WindowView.Create(logWindow);

            Object.DontDestroyOnLoad(_view);
            Object.DontDestroyOnLoad(_logView);

    
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
            ILog cachedLog, 
            IFileSystemFactory fsFactory, 
            IAssemblyProvider<ITypeIdentifier> partModuleProxyProvider,
            DefaultAssemblyResolver assemblyResolver)
        {
            var reloadableAssemblyFileQuery = new ReloadableAssemblyFilesInDirectoryQuery(fsFactory.GetGameDataDirectory());
            var partModulesFromAssemblyQuery = new PartModulesFromAssemblyQuery();

            var destructionMediator = new GameObjectDestroyForReload();

            var addonFactory = new AddonFactory(destructionMediator, cachedLog.CreateTag("AddonFactory"), new AddonAttributeFromTypeQuery());
            var descriptorFactory = new PartModuleDescriptorFactory(
                new KspPartLoader(new KspFactory()),
                new AvailablePartConfigProvider(new KspGameDatabase()),
                new ModuleConfigsFromPartConfigQuery(),
                new TypeIdentifierQuery(),
                _log.CreateTag("DescriptorFactory"));

            var flightConfigRepository = new PartModuleFlightConfigRepository();
            var loadedPrefabProvider = new LoadedInstancesOfPrefabProvider(new LoadedVesselProvider(new KspFactory()));

            var ilModifications = ConfigureAssemblyModifications();


            return reloadableAssemblyFileQuery.Get().Select(raFile =>
            {
                var addonLoader = new Loaders.AddonLoader.AddonLoader(addonFactory, new AddonsFromAssemblyQuery(new AddonAttributeFromTypeQuery()), new CurrentStartupSceneProvider(new StartupSceneFromGameSceneQuery(), new CurrentGameSceneProvider()), cachedLog);
                var pmFactory = new PartModuleFactory(new ProxyPartModuleTypeProvider(partModuleProxyProvider, partModulesFromAssemblyQuery, new TypeIdentifierQuery()));
                var pmLoader = new PartModuleLoader(
                    pmFactory,
                   
                    flightConfigRepository,
                    loadedPrefabProvider);

                var partModuleController = new PartModuleController(
                    pmLoader,
                    partModulesFromAssemblyQuery,
                    descriptorFactory,
                    new CurrentSceneIsFlightQuery());


                _eventProvider.OnSceneLoaded.OnEvent += addonLoader.CreateForScene;

                IReloadablePlugin plugin = new ReloadablePlugin(
                    ConfigureAssemblyProvider(raFile, assemblyResolver, ilModifications));

                plugin.OnLoaded += addonLoader.LoadAddonTypes;
                plugin.OnLoaded += partModuleController.LoadPartModules;

                plugin.OnUnloaded += (f => addonLoader.ClearAddonTypes(true));
                plugin.OnUnloaded += partModuleController.UnloadPartModules;

                return plugin;
            });
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


        private IAssemblyProvider ConfigureAssemblyProvider(IFile location, BaseAssemblyResolver resolver, ICommand<AssemblyDefinition> ilModifications)
        {
            if (location == null) throw new ArgumentNullException("location");
            if (resolver == null) throw new ArgumentNullException("resolver");
            if (ilModifications == null) throw new ArgumentNullException("ilModifications");

            return new AssemblyProvider(
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
    }
}
