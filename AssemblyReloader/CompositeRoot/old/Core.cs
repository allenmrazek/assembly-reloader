//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Reflection;
//using AssemblyReloader.Annotations;
//using AssemblyReloader.Commands;
//using AssemblyReloader.Controllers;
//using AssemblyReloader.DataObjects;
//using AssemblyReloader.Game;
//using AssemblyReloader.Game.Providers;
//using AssemblyReloader.Game.Queries;
//using AssemblyReloader.Generators;
//using AssemblyReloader.Loaders;
//using AssemblyReloader.Loaders.PartModuleLoader;
//using AssemblyReloader.Loaders.ScenarioModuleLoader;
//using AssemblyReloader.Providers;
//using AssemblyReloader.Queries;
//using AssemblyReloader.Queries.AssemblyQueries;
//using AssemblyReloader.Queries.CecilQueries;
//using AssemblyReloader.Queries.FileSystemQueries;
//using AssemblyReloader.TypeInstallers;
//using AssemblyReloader.Weaving;
//using AssemblyReloader.Weaving.Operations;
//using Mono.Cecil;
//using Mono.Cecil.Cil;
//using ReeperCommon.Containers;
//using ReeperCommon.Extensions;
//using ReeperCommon.FileSystem;
//using ReeperCommon.Logging;
//using ReeperCommon.Repositories;
//using ReeperCommon.Serialization;
//using UnityEngine;

//namespace AssemblyReloader.CompositeRoot
//{
//    // composite root
//    class Core
//    {
//        private readonly ILog _log;


//        private interface IConsumer
//        {
//            void Consume(object message);
//        }


//        private class MessageChannel : IMessageChannel
//        {
//            private readonly List<IConsumer> _consumers;

//            public MessageChannel(params IConsumer[] consumers)
//            {
//                _consumers = new List<IConsumer>(consumers);
//            }


//            public void Send<T>(T message)
//            {
//                _consumers.ForEach(consumer => consumer.Consume(message));
//            }

//            public void AddListener<T>(object listener)
//            {
//                AddConsumer(new Consumer<T>(listener));
//            }

//            public void RemoveListener(object listener)
//            {
//                _consumers.RemoveAll(ic => ReferenceEquals(ic, listener));
//            }


//            private void AddConsumer(IConsumer consumer)
//            {
//                if (consumer == null) throw new ArgumentNullException("consumer");

//                if (!_consumers.Contains(consumer))
//                    _consumers.Add(consumer);
//            }


//        }





//        private class Consumer<T> : IConsumer
//        {
//            private readonly object _consumer;

//            public Consumer(object consumer)
//            {
//                if (consumer == null) throw new ArgumentNullException("consumer");
//                if (!(consumer is IMessageConsumer<T>)) throw new InvalidOperationException("consumer is not a " + typeof (T).Name);

//                _consumer = consumer;
//            }


//            public void Consume(object message)
//            {
//                if (message is T && _consumer is IMessageConsumer<T>)
//                    (_consumer as IMessageConsumer<T>).Consume((T)message);
//            }
//        }


//        private class EventProvider : IEventProvider
//        {
//            public IGameEventPublisher<GameScenes> OnLevelWasLoaded { get; set; }
//            public IGameEventPublisher<KSPAddon.Startup> OnSceneLoaded { get; set; }
//        }



//        public Core()
//        {
//            //var container = TinyIoCContainer.Current;

//#if DEBUG
//            //var primaryLog = new DebugLog("ART");
//            //container.Register<ILog>(new DebugLog("ART"), "MainLog");
//#else
//            var primaryLog = LogFactory.Create(LogLevel.Standard);
//            //container.Register<ILog>(LogFactory.Create(LogLevel.Standard), "MainLog");
//#endif

//            //container.Register<ILog>(_log);
//            //container.Register<ILog>(_log, "MainLog");

//            //container.AutoRegister(DuplicateImplementationActions.RegisterSingle);

//            //container.Register<RandomStringGenerator>().AsSingleton();
//            //container.Register<IUrlDirProvider>(new KSPGameDataUrlDirProvider(), "GameData");

//            //container.Register<IFileSystemFactory>(new KSPFileSystemFactory(
//            //    new KSPUrlDir(container.Resolve<IUrlDirProvider>("GameData").Get())));

//            //container.Register<IAssemblyFileLocationQuery>(
//            //    (cContainer, overloads) => cContainer.Resolve<AssemblyFileLocationQuery>(overloads));

//            //var mainAssemblyFile = container.Resolve<IAssemblyFileLocationQuery>().Get(Assembly.GetExecutingAssembly());
//            //    if (!mainAssemblyFile.Any()) throw new Exception("Failed to locate executing assembly file!");

//            //container.Register<IConfigNodeSerializer>(
//            //    new ConfigNodeSerializer(new DefaultSurrogateSelector(new DefaultSurrogateProvider()),
//            //        new CompositeFieldInfoQuery(new RecursiveSerializableFieldQuery())));


//            //container.Register(container.Resolve<IFileSystemFactory>().GetGameDataDirectory(), "GameData");
//            //container.Register<IGameAddonLoader>(new KspAddonLoader());
//            //container.Register<IUnityObjectDestroyer>(
//            //    new UnityObjectDestroyer(new PluginReloadRequestedMethodCallCommand()));
//            //container.Register<IAddonAttributesFromTypeQuery>(new AddonAttributesFromTypeQuery());

//            //var ourDirProvider =
//            //    container.Resolve<AssemblyDirectoryQuery>(new NamedParameterOverloads
//            //    {
//            //        { "assembly", Assembly.GetExecutingAssembly() },
//            //        { "gameData", container.Resolve<IDirectory>("GameData") }
//            //    });

//            //container.Register(ourDirProvider.Get(), "Core");
//            //container.Register(ConfigureResourceRepository(container.Resolve<IDirectory>("Core")));
//            //container.Register(ConfigureEventProvider(container.Resolve<IStartupSceneFromGameSceneQuery>()));
//            //container.Register(ConfigureLoadedAssemblyFactory());
//            //container.Register(mainAssemblyFile.Single(), "Core");
//            //container.Register<IFilePathProvider>(new ConfigFilePathProvider(container.Resolve<IFile>("Core")), "Core");
//            //container.Register(container.Resolve<ConfigurationProvider>(new NamedParameterOverloads
//            //{
//            //    { "configurationFileProvider", container.Resolve<IFilePathProvider>("Core") },
//            //    { "log", container.Resolve<ILog>("MainLog") }
//            //}).Get());

//            //var assemblyResolver = new DefaultAssemblyResolver();
//            //assemblyResolver.AddSearchDirectory(container.Resolve<IDirectory>("Core").FullPath);

//            //container.Register<BaseAssemblyResolver, DefaultAssemblyResolver>(assemblyResolver);
//            //container.Register<IPluginConfigurationFilePathQuery>(new PluginConfigurationFilePathQuery());
//            //container.Register<IPartLoader, KspPartLoader>().AsSingleton();
//            //container.Register<IAvailablePartConfigQuery, AvailablePartConfigQuery>().AsSingleton();
//            //container.Register<IModuleConfigsFromPartConfigQuery, ModuleConfigsFromPartConfigQuery>().AsSingleton();
//            //container.Register<ITypeIdentifierQuery, TypeIdentifierQuery>().AsSingleton();
//            //container.Register<IGetLoadedUnityComponents<Part>>(new getLoadedUnityComponents<Part>());
//            //container.Register<IComponentsInGameObjectHierarchyProvider<Part>>(
//            //    new ComponentsInGameObjectHierarchyProvider<Part>());

//            //container.Register<IPartPrefabCloneProvider, PartPrefabCloneProvider>(
//            //    container.Resolve<PartPrefabCloneProvider>());

//            //container.Register<IPartModuleFactory, PartModuleFactory>(
//            //    container.Resolve<PartModuleFactory>(new NamedParameterOverloads
//            //    {
//            //        { "awakenPartModule", new AwakenPartModuleCommand() },
//            //        { "onStartRunner", container.Resolve<ExecutePartModuleOnStartsCommand>(
//            //            new NamedParameterOverloads()
//            //            {
//            //                { "log", container.Resolve<ILog>("MainLog").CreateTag("PartModuleOnStartRunner") }
//            //            }) }
//            //    }));

//            //if (!container.CanResolve<ConfigurationProvider>())
//            //    throw new Exception("Couldn't resolve IConfigurationProvider");

//            //container.Register<IScenarioModuleLoader, ScenarioModuleLoader>().AsMultiInstance();
//            //container.Register<IScenarioModuleUnloader, ScenarioModuleUnloader>().AsMultiInstance();
//            //container.Register<IGameObjectProvider, KspGameObjectProvider>().AsSingleton();
//            //container.Register<IScenarioModuleSnapshotGenerator, ScenarioModuleSnapshotGenerator>().AsMultiInstance();
//            //container.Register<ConfigurationProvider>((c, p) => c.Resolve<ConfigurationProvider>(new NamedParameterOverloads { { "log", c.Resolve<ILog>("MainLog") } }));
//            //container.Register<IConfigurationProvider>((cContainer, overloads) => cContainer.Resolve<ConfigurationProvider>(new NamedParameterOverloads { { "log", cContainer.Resolve<ILog>("MainLog") }}));
//            ////(cContainer, nOverloads) => cContainer.Resolve<ConfigurationProvider>(new NamedParameterOverloads { { "log", cContainer.Resolve<ILog>("MainLog") }}));
//            ////container.Register<IConfigurationProvider, ConfigurationProvider>().AsMultiInstance();



//            //_log = container.Resolve<ILog>("MainLog");

//            //KspPartActionWindowListener.WindowController = new KspPartActionWindowController();
//            //KspPartActionWindowListener.PartActionWindowQuery =
//            //    new ComponentsInGameObjectHierarchyProvider<UIPartActionWindow>();


//            //var eventProvider = container.Resolve<IEventProvider>();

//            //eventProvider.OnSceneLoaded.OnEvent += s =>
//            //{
//            //    if (UIPartActionController.Instance != null && UIPartActionController.Instance.windowPrefab != null)
//            //        if (UIPartActionController.Instance.windowPrefab.GetComponent<KspPartActionWindowListener>() == null)
//            //            UIPartActionController.Instance.windowPrefab.gameObject.AddComponent<KspPartActionWindowListener>();
//            //};


//            //var reloadables = CreateReloadablePlugins(container).ToList();

//            //reloadables.ForEach(r => r.Load());

//        }


//        [UsedImplicitly]
//        public void Tick()
//        {
            
//        }


//        //private void CreateWindows(
//        //    [NotNull] Configuration configuration, 
//        //    [NotNull] List<ReloadablePlugin> reloadables, 
//        //    [NotNull] IConfigNodeSerializer serializer, [NotNull] IResourceRepository resourceLocator,
//        //    [NotNull] IFilePathProvider mainConfigPathProvider)
//        //{
//        //    if (configuration == null) throw new ArgumentNullException("configuration");
//        //    if (reloadables == null) throw new ArgumentNullException("reloadables");
//        //    if (serializer == null) throw new ArgumentNullException("serializer");
//        //    if (resourceLocator == null) throw new ArgumentNullException("resourceLocator");
//        //    if (mainConfigPathProvider == null) throw new ArgumentNullException("mainConfigPathProvider");


//        //    var skinScheme = ConfigureSkin(resourceLocator);

//        //    var btnCloseTexture = resourceLocator.GetTexture("Resources/btnClose.png");
//        //    if (!btnCloseTexture.Any()) throw new Exception("Failed to find window close button texture!");

//        //    var btnOptionsTexture = resourceLocator.GetTexture("Resources/btnWrench.png");
//        //    if (!btnOptionsTexture.Any()) throw new Exception("Failed to find window option button texture!");

//        //    var btnResizeCursorTexture = resourceLocator.GetTexture("Resources/cursor.png");
//        //    if (!btnResizeCursorTexture.Any()) throw new Exception("Failed to find window resize cursor texture!");

//        //    var mainConfigNode = new ConfigNode("AssemblyReloaderConfig");

//        //    var doConfigurationSerializeCommand = new SerializeObjectCommand(serializer, mainConfigNode);

//        //    var serializeConfigurationCommand = new ContextProviderCommand<SerializationContext>(
//        //        () => new SerializationContext(configuration, mainConfigPathProvider), 
//        //        new CompositeCommand<SerializationContext>(new Command<SerializationContext>(c => mainConfigNode.ClearData()),
//        //        doConfigurationSerializeCommand));

//        //    //var doPluginConfigurationSerializeCommand = new SerializeObjectCommand(serializer,
//        //    //    "AssemblyReloaderPluginConfiguration");

//        //    //var serializePluginConfigurationCommand =
//        //    //    new ContextAdapter<IPluginInfo, SerializationContext>(
//        //    //        pluginInfo =>
//        //    //            new SerializationContext(pluginInfo.Configuration,
//        //    //                new ConfigFilePathProvider(pluginInfo.Location)),
//        //    //        doPluginConfigurationSerializeCommand);


//        //    var mediator =
//        //        new Controllers.Controller(
//        //            reloadables.ToDictionary(r => r as IPluginInfo, r => r as IReloadablePlugin),
//        //            serializeConfigurationCommand,
//        //            /*serializePluginConfigurationCommand*/ new NullCommand<IPluginInfo>(),
//        //            _log.CreateTag("Controller"));

//        //    var viewMessageChannel = new MessageChannel();

//        //    var windowFactory = new WindowFactory(new UniqueWindowIdProvider(), mediator, viewMessageChannel,
//        //        ConfigureTitleBarButtonStyle(), btnOptionsTexture.Single(), btnCloseTexture.Single());

//        //    var mainAppearance = new WindowAppearanceInfo(skinScheme,
//        //        new Rect(200f, 200f, 400f, 200f), new Vector2(10f, 10f), new Vector2(150f, 100f),
//        //        btnResizeCursorTexture.Single());

//        //    var windowDescriptors = new List<WindowDescriptor>();

//        //    try
//        //    {
//        //        var mainWindow = windowFactory.CreateMainWindow(reloadables.Cast<IPluginInfo>(), mainAppearance,
//        //            Maybe<ConfigNode>.None);
//        //        windowDescriptors.Add(mainWindow);

//        //        doConfigurationSerializeCommand.OnSerialized += node =>
//        //        {
//        //            var windowCfg = new ConfigNode("MainWindow");
//        //            mainWindow.DecoratedLogic.Serialize(serializer, windowCfg);
//        //        };

//        //        var optionsWindow = windowFactory.CreateOptionsWindow(mainAppearance, configuration, Maybe<ConfigNode>.None);
//        //        windowDescriptors.Add(optionsWindow);
//        //        viewMessageChannel.AddListener<ShowConfigurationWindow>(optionsWindow.BaseLogic);

//        //        reloadables.ForEach(r =>
//        //        {
//        //            var pluginConfigWindow = windowFactory.CreatePluginOptionsWindow(mainAppearance, r,
//        //                Maybe<ConfigNode>.None);
//        //            windowDescriptors.Add(pluginConfigWindow);

//        //            //doPluginConfigurationSerializeCommand.OnSerialized += node =>
//        //            //{
//        //            //    var optionsCfg = new ConfigNode("Window");
//        //            //    pluginConfigWindow.DecoratedLogic.Serialize(serializer, optionsCfg);
//        //            //};

//        //            viewMessageChannel.AddListener<ShowPluginConfigurationWindow>(pluginConfigWindow.BaseLogic);
//        //        });


//        //    }
//        //    catch (Exception)
//        //    {
//        //        _log.Error("Encountered an exception while creating windows");

//        //        // need to destroy the windows, otherwise they'll stick around on unhandled exceptions
//        //        windowDescriptors.ForEach(d => Object.Destroy(d.View));

//        //        throw;
//        //    }
//        //}



//        private IResourceRepository ConfigureResourceRepository(IDirectory dllDirectory)
//        {
//            return new ResourceRepositoryComposite(
//                // search GameDatabase first
//                //   note: GameDatabase expects extensionless urls
//                    new ResourceIdentifierAdapter(id =>
//                    {
//                        if (!Path.HasExtension(id) || string.IsNullOrEmpty(id)) return id;

//                        var dir = Path.GetDirectoryName(id) ?? "";
//                        var woExt = Path.Combine(dir, Path.GetFileNameWithoutExtension(id)).Replace('\\', '/');

//                        return !string.IsNullOrEmpty(woExt) ? woExt : id;
//                    }, new ResourceFromGameDatabase()
//                    ),

//                // then look at physical file system. These work on a list of items cached
//                // by GameDatabase rather than working directly with the disk (unless a resource 
//                // is accessed from here, of course)
//                    new ResourceFromDirectory(dllDirectory),


//                // finally search embedded resource
//                //   note: embedded resource ids should be appended by assembly namespace
//                    new ResourceIdentifierAdapter(id => Assembly.GetExecutingAssembly().GetName().Name + "." + id,

//                //   note: expects resource manifest ids (periods instead of slashes), so 
//                //         identifier transformer is needed if we want to specify input identifiers
//                //         as though they were paths
//                    new ResourceIdentifierAdapter(id => id.Replace('/', '.').Replace('\\', '.'),
//                        new ResourceFromEmbeddedResource(Assembly.GetExecutingAssembly()))
//                    ));
//        }


//        private ILoadedAssemblyFactory ConfigureLoadedAssemblyFactory()
//        {
//            return new KspLoadedAssemblyFactory(
//                new GetLoadedAssemblyFileUrl(),
//                new LoadedAssemblyHandleFactory(),
//                // note: no KSPAddon type installer required; game looks through LoadedAssemblies on every scene checking all types
//                new GenericLoadedAssemblyTypeInstaller<Part>(new GetTypesDerivedFrom<Part>()),
//                new GenericLoadedAssemblyTypeInstaller<PartModule>(new GetTypesDerivedFrom<PartModule>()),
//                new GenericLoadedAssemblyTypeInstaller<ScenarioModule>(new GetTypesDerivedFrom<ScenarioModule>()));
//        }


//        //private IEnumerable<ReloadablePlugin> CreateReloadablePlugins(TinyIoCContainer container)
//        //{
//        //    var reloadableAssemblyFilesInDirectoryQuery =
//        //        container.Resolve<ReloadableAssemblyFilesInDirectoryQuery>(new NamedParameterOverloads { {"topDirectory", container.Resolve<IDirectory>("GameData") } });

//        //    return reloadableAssemblyFilesInDirectoryQuery.Get().Select(f => ConfigureReloadablePlugin(container, f));
//        //}


//        private static IEventProvider ConfigureEventProvider(IStartupSceneFromGameSceneQuery query)
//        {
//            if (query == null) throw new ArgumentNullException("query");

//            var onLevelWasLoaded = new GameEventPublisher<GameScenes>();
//            GameEvents.onLevelWasLoaded.Add(onLevelWasLoaded.Raise);

//            var onSceneLoaded = new GameEventPublisher<KSPAddon.Startup>();
//            onLevelWasLoaded.OnEvent += gameScene => onSceneLoaded.Raise(query.Get(gameScene));

//            return new EventProvider{
//                OnLevelWasLoaded = onLevelWasLoaded,
//                OnSceneLoaded = onSceneLoaded
//            };
//        }


//        //private ReloadablePlugin ConfigureReloadablePlugin(
//        //    [NotNull] TinyIoCContainer container,
//        //    [NotNull] IFile location)
//        //{
//        //    if (container == null) throw new ArgumentNullException("container");
//        //    if (location == null) throw new ArgumentNullException("location");

//        //    var configuration = new PluginConfigurationProvider(container.Resolve<IPluginConfigurationFilePathQuery>()).Get(location);

//        //    var debugSymbolExistQuery = new DebugSymbolFileExistsQuery(location);

//        //    var assemblyProvider = new AssemblyProvider(
//        //        new AssemblyDefinitionFromDiskReader(
//        //            location,
//        //            debugSymbolExistQuery,
//        //            container.Resolve<BaseAssemblyResolver>()),
//        //        new ConditionalWriteLoadedAssemblyToDisk(
//        //            new AssemblyDefinitionLoader(
//        //                new TemporaryFileFactory(
//        //                    location.Directory,
//        //                    new RandomStringGenerator()),
//        //                _log.CreateTag("AssemblyDefinitionLoader")),
//        //            () => configuration.WriteReweavedAssemblyToDisk,
//        //            location.Directory),
//        //        ConfigureDefinitionWeaver(location, configuration));

//        //    var loader = new Loaders.AssemblyLoader(assemblyProvider, container.Resolve<ILoadedAssemblyFactory>(),
//        //        container.Resolve<ILog>("MainLog").CreateTag("AssemblyLoader"));

//        //    var reloadable = new ReloadablePlugin(loader, location, configuration);

//        //    SetupAddonController(container, reloadable);
//        //    SetupPartModuleController(container, reloadable);
//        //    SetupScenarioModuleController(container, reloadable);

//        //    return reloadable;
//        //}


//        //private static void SetupAddonController(
//        //    [NotNull] TinyIoCContainer container,
//        //    [NotNull] ReloadablePlugin plugin)
//        //{
//        //    if (container == null) throw new ArgumentNullException("container");
//        //    if (plugin == null) throw new ArgumentNullException("plugin");

//        //    var addonLoader = new Loaders.AddonLoader(
//        //        container.Resolve<IGameAssemblyLoader>(),
//        //        container.Resolve<IGameAddonLoader>(),
//        //        container.Resolve<IGetCurrentStartupScene>(),
//        //        () => plugin.Configuration.InstantlyAppliesToEveryScene);

//        //    var addonUnloader = container.Resolve<AddonUnloader>(new NamedParameterOverloads
//        //    {
//        //        { "addonGetTypesFromAssembly", new AddonsFromAssembly(container.Resolve<IAddonAttributesFromTypeQuery>()) }
//        //    });

//        //    var addonController = new AddonFacade(addonLoader, addonUnloader);

//        //    plugin.OnLoaded +=
//        //        (asm, loc) => { if (plugin.Configuration.StartAddonsForCurrentScene) addonController.Load(asm, loc); };

//        //    plugin.OnUnloaded += addonController.Unload;
//        //}

//        //private static void SetupAddonController(
//        //    [NotNull] ReloadablePlugin plugin, 
//        //    [NotNull] IGameAssemblyLoader gameAssemblyLoader,
//        //    [NotNull] IGameAddonLoader gameAddonLoader,
//        //    [NotNull] IUnityObjectDestroyer objectDestroyer)

//        //{
//        //    if (plugin == null) throw new ArgumentNullException("plugin");
//        //    if (gameAssemblyLoader == null) throw new ArgumentNullException("gameAssemblyLoader");
//        //    if (gameAddonLoader == null) throw new ArgumentNullException("gameAddonLoader");
//        //    if (objectDestroyer == null) throw new ArgumentNullException("objectDestroyer");

//        //    var addonLoader = new Loaders.AddonLoader(
//        //        gameAssemblyLoader,
//        //        gameAddonLoader,
//        //        new GetCurrentStartupScene(new StartupSceneFromGameSceneQuery(), new CurrentGameSceneProvider()),
//        //        () => plugin.Configuration.InstantlyAppliesToEveryScene);

//        //    var addonUnloader = new AddonUnloader(
//        //        new AddonsFromAssembly(new AddonAttributesFromTypeQuery()),
//        //        objectDestroyer,
//        //        new getLoadedUnityComponents());

//        //    var addonController = new AddonFacade(addonLoader, addonUnloader);

//        //    plugin.OnLoaded +=
//        //        (asm, loc) => { if (plugin.Configuration.StartAddonsForCurrentScene) addonController.Load(asm, loc); };

//        //    plugin.OnUnloaded += addonController.Unload; 
//        //}


//        //private void SetupPartModuleController(TinyIoCContainer container, ReloadablePlugin plugin)
//        //{
//        //    var partModuleConfigQueue = new DictionaryQueue<KeyValuePair<uint, ITypeIdentifier>, ConfigNode>(
//        //        new FlightConfigNodeKeyValuePairComparer());

//        //    //var descriptorFactory = container.Resolve<PartModuleDescriptorFactory>();
//        //    //var prefabCloneProvider = container.Resolve<PartPrefabCloneProvider>();

//        //    Func<bool> reuseConfigNodes = () => plugin.Configuration.ReusePartModuleConfigsFromPrevious;

//        //    var onStartRunner = container.Resolve<ExecutePartModuleOnStartsCommand>(new NamedParameterOverloads
//        //    {
//        //        { "log", container.Resolve<ILog>("MainLog").CreateTag(plugin.Name + " OnStart Runner") }
//        //    });

//        //    //var partModuleFactory = container.Resolve<PartModuleFactory>(new NamedParameterOverloads
//        //    //{
//        //    //    { "awakenPartModule", new AwakenPartModuleCommand() },
//        //    //    { "onStartRunner", onStartRunner }
//        //    //});

//        //    var partModuleLoader = container.Resolve<PartModuleLoader>(new NamedParameterOverloads()
//        //    {
//        //        { "configNodeQueue", partModuleConfigQueue },
//        //        { "useConfigNodeSnapshotIfAvailable", reuseConfigNodes }
//        //    });

//        //    var partModuleUnloader = container.Resolve<PartModuleUnloader>(new NamedParameterOverloads
//        //    {
//        //        { "snapshotGenerator", ConfigurePartModuleSnapshotGenerator(partModuleConfigQueue) }
//        //    });

//        //    var partModuleController = container.Resolve<PartModuleFacade>(new NamedParameterOverloads
//        //    {
//        //        { "pmLoader", partModuleLoader },
//        //        { "pmUnloader", partModuleUnloader },
//        //        { "partModuleFromAssemblyQuery", new GetTypesDerivedFrom<PartModule>() },
//        //        { "onPartModulesLoaded", new CompositeCommand(
//        //                                        onStartRunner,
//        //                                        new ClearDictionaryQueryCommand<KeyValuePair<uint, ITypeIdentifier>, ConfigNode>(partModuleConfigQueue), 
//        //                                        new RefreshPartActionWindows(KspPartActionWindowListener.WindowController)) },
//        //        { "onPartModulesUnloaded", new NullCommand() }
//        //    });

//        //    plugin.OnLoaded += (asm, loc) =>
//        //    {
//        //        onStartRunner.ClearPartModuleTargets();
//        //        if (plugin.Configuration.ReloadPartModulesImmediately) partModuleController.Load(asm, loc);
//        //    };

//        //    plugin.OnUnloaded += partModuleController.Unload;

//        //    //    var descriptorFactory = new PartModuleDescriptorFactory(
//        //    //                                new KspPartLoader(
//        //    //                                    kspFactory),
//        //    //                                new AvailablePartConfigQuery(
//        //    //                                    new KspGameDatabase()),
//        //    //                                new ModuleConfigsFromPartConfigQuery(),
//        //    //                                new TypeIdentifierQuery());

//        //    //    var prefabCloneProvider = new PartPrefabCloneProvider(
//        //    //                                new getLoadedUnityComponents<Part>(),
//        //    //                                new ComponentsInGameObjectHierarchyProvider<Part>(),
//        //    //                                new PartIsPrefabQuery(),
//        //    //                                kspFactory);

//        //    //    Func<bool> reuseConfigNodes = () => plugin.Configuration.ReusePartModuleConfigsFromPrevious;

//        //    //    var onStartRunner = new ExecutePartModuleOnStartsCommand(new PartModuleStartStateProvider(),
//        //    //        new PartIsPrefabQuery(), kspFactory, _log.CreateTag(plugin.Name + " OnStart Runner"));

//        //    //    var partModuleController = new PartModuleFacade(
//        //    //                                 new PartModuleLoader(
//        //    //                                     descriptorFactory,
//        //    //                                     new PartModuleFactory(new PartIsPrefabQuery(), new AwakenPartModuleCommand(), onStartRunner),
//        //    //                                     partModuleConfigQueue,
//        //    //                                     prefabCloneProvider,
//        //    //                                     reuseConfigNodes),
//        //    //                                 new PartModuleUnloader(
//        //    //                                     new UnityObjectDestroyer(new PluginReloadRequestedMethodCallCommand()),
//        //    //                                     descriptorFactory,
//        //    //                                     prefabCloneProvider,
//        //    //                                     ConfigurePartModuleSnapshotGenerator(partModuleConfigQueue),
//        //    //                                     reuseConfigNodes
//        //    //                                     ),
//        //    //                                 new GetTypesDerivedFrom<PartModule>(),
//        //    //                                 new CompositeCommand(
//        //    //                                    onStartRunner,
//        //    //                                    new ClearDictionaryQueryCommand<KeyValuePair<uint, ITypeIdentifier>, ConfigNode>(partModuleConfigQueue), 
//        //    //                                    new RefreshPartActionWindows(KspPartActionWindowListener.WindowController)),
//        //    //                                    new NullCommand());

//        //    //    plugin.OnLoaded += (asm, loc) =>
//        //    //    {
//        //    //        onStartRunner.ClearPartModuleTargets();
//        //    //        if (plugin.Configuration.ReloadPartModulesImmediately) partModuleController.Load(asm, loc);
//        //    //    };

//        //    //    plugin.OnUnloaded += partModuleController.Unload;
//        //}


//        //private void SetupPartModuleController(ReloadablePlugin plugin, IKspFactory kspFactory)
//        //{
//        //    var partModuleConfigQueue = new DictionaryQueue<KeyValuePair<uint, ITypeIdentifier>, ConfigNode>(
//        //        new FlightConfigNodeKeyValuePairComparer()); 
            

//        //    var descriptorFactory = new PartModuleDescriptorFactory(
//        //                                new KspPartLoader(
//        //                                    kspFactory),
//        //                                new AvailablePartConfigQuery(
//        //                                    new KspGameDatabase()),
//        //                                new ModuleConfigsFromPartConfigQuery(),
//        //                                new TypeIdentifierQuery());

//        //    var prefabCloneProvider = new PartPrefabCloneProvider(
//        //                                new getLoadedUnityComponents<Part>(),
//        //                                new ComponentsInGameObjectHierarchyProvider<Part>(),
//        //                                new PartIsPrefabQuery(),
//        //                                kspFactory);

//        //    Func<bool> reuseConfigNodes = () => plugin.Configuration.ReusePartModuleConfigsFromPrevious;

//        //    var onStartRunner = new ExecutePartModuleOnStartsCommand(new PartModuleStartStateProvider(),
//        //        new PartIsPrefabQuery(), kspFactory, _log.CreateTag(plugin.Name + " OnStart Runner"));

//        //    var partModuleController = new PartModuleFacade(
//        //                                 new PartModuleLoader(
//        //                                     descriptorFactory,
//        //                                     new PartModuleFactory(new PartIsPrefabQuery(), new AwakenPartModuleCommand(), onStartRunner),
//        //                                     partModuleConfigQueue,
//        //                                     prefabCloneProvider,
//        //                                     reuseConfigNodes),
//        //                                 new PartModuleUnloader(
//        //                                     new UnityObjectDestroyer(new PluginReloadRequestedMethodCallCommand()),
//        //                                     descriptorFactory,
//        //                                     prefabCloneProvider,
//        //                                     ConfigurePartModuleSnapshotGenerator(partModuleConfigQueue),
//        //                                     reuseConfigNodes
//        //                                     ),
//        //                                 new GetTypesDerivedFrom<PartModule>(),
//        //                                 new CompositeCommand(
//        //                                    onStartRunner,
//        //                                    new ClearDictionaryQueryCommand<KeyValuePair<uint, ITypeIdentifier>, ConfigNode>(partModuleConfigQueue), 
//        //                                    new RefreshPartActionWindows(KspPartActionWindowListener.WindowController)),
//        //                                    new NullCommand());

//        //    plugin.OnLoaded += (asm, loc) =>
//        //    {
//        //        onStartRunner.ClearPartModuleTargets();
//        //        if (plugin.Configuration.ReloadPartModulesImmediately) partModuleController.Load(asm, loc);
//        //    };

//        //    plugin.OnUnloaded += partModuleController.Unload;
//        //}


//        //private void SetupScenarioModuleController(IReloadablePlugin plugin, PluginConfiguration pluginConfiguration, IKspFactory kspFactory)
//        //{
//        //    var gameProvider = new CurrentGameProvider(new TypeIdentifierQuery());
//        //    var currentGameSceneProvider = new CurrentGameSceneProvider();

//        //    var protoScenarioModuleProvider = new ProtoScenarioModuleProvider(
//        //        kspFactory,
//        //        new TypeIdentifierQuery(),
//        //        new CurrentGameProvider(new TypeIdentifierQuery()),
//        //        currentGameSceneProvider);

//        //    var scenarioModuleController =
//        //        new ScenarioModuleFacade(
//        //            new ScenarioModuleLoader(protoScenarioModuleProvider),
//        //            new ScenarioModuleUnloader(
//        //                new GameObjectComponentQuery(new KspGameObjectProvider()),
//        //                protoScenarioModuleProvider,
//        //                new UnityObjectDestroyer(new PluginReloadRequestedMethodCallCommand()),
//        //                () => pluginConfiguration.SaveScenarioModuleConfigBeforeReloading,
//        //                new ScenarioModuleSnapshotGenerator(gameProvider, _log.CreateTag("SMSnapshotGen")),
//        //                _log.CreateTag("ScenarioModuleUnloader")),
//        //            new GetTypesDerivedFrom<ScenarioModule>(),
//        //            currentGameSceneProvider);

//        //    plugin.OnLoaded += (asm, loc) =>
//        //    {
//        //        if (pluginConfiguration.ReloadScenarioModulesImmediately) scenarioModuleController.Load(asm, loc);
//        //    };

//        //    plugin.OnUnloaded += scenarioModuleController.Unload;
//        //}


//        //private void SetupScenarioModuleController(TinyIoCContainer container, ReloadablePlugin plugin)
//        //{
//        //    var scenarioModuleUnloader = new ScenarioModuleUnloader(
//        //        container.Resolve<IGameObjectComponentQuery>(),
//        //        container.Resolve<IProtoScenarioModuleProvider>(),
//        //        container.Resolve<IUnityObjectDestroyer>(),
//        //        () => plugin.Configuration.SaveScenarioModuleConfigBeforeReloading,
//        //        container.Resolve<IScenarioModuleSnapshotGenerator>(),
//        //        container.Resolve<ILog>("MainLog").CreateTag("ScenarioModuleUnloader"));

//        //    var scenarioModuleController = container.Resolve<ScenarioModuleFacade>(new NamedParameterOverloads
//        //    {
//        //        { "scenarioModuleQuery", container.Resolve<IGetTypesDerivedFrom<ScenarioModule>>() },
//        //        { "unloader", scenarioModuleUnloader}
//        //    });

//        //    plugin.OnLoaded += (asm, loc) =>
//        //    {
//        //        if (plugin.Configuration.ReloadScenarioModulesImmediately) scenarioModuleController.Load(asm, loc);
//        //    };

//        //    plugin.OnUnloaded += scenarioModuleController.Unload;
//        //}

//        private GUISkin ConfigureSkin(IResourceRepository resources)
//        {
//            Resources.FindObjectsOfTypeAll<Font>()
//                .ToList()
//                .ForEach(
//                    f =>
//                        new DebugLog().Normal("ConfigureSkin: font = " +
//                                              f.fontNames.Aggregate(string.Empty, (s1, s2) => s1 + ", " + s2)));

//            //Resources.FindObjectsOfTypeAll<GUISkin>().ToList().ForEach(s => new DebugLog().Debug("Skin: " + s.name));

//            //UnityEngine.Object.FindObjectOfType<AssetBase>()
//            //    .guiSkins.ToList()
//            //    .ForEach(g => new DebugLog().Debug("AB: Skin: " + g.name));

//            //var skin = UnityEngine.Object.Instantiate(HighLogic.Skin) as GUISkin;
//            //var skin = UnityEngine.Object.Instantiate(AssetBase.GetGUISkin("KSP window 5")) as GUISkin;
//            //var skin = UnityEngine.Object.Instantiate(AssetBase.GetGUISkin("KSP window 4")) as GUISkin;
//            //var skin = UnityEngine.Object.Instantiate(AssetBase.GetGUISkin("KSP window 4")) as GUISkin;
//            //var skin = UnityEngine.Object.Instantiate(AssetBase.GetGUISkin("KSP window 3")) as GUISkin;
//            //var skin = UnityEngine.Object.Instantiate(AssetBase.GetGUISkin("KSP window 1")) as GUISkin;
//            var skin = UnityEngine.Object.Instantiate(AssetBase.GetGUISkin("OrbitMapSkin")) as GUISkin;
//            if (skin == null) throw new Exception("Failed to clone OrbitMapSkin");

//            skin.font = Resources.FindObjectsOfTypeAll<Font>().FirstOrDefault(f => f.fontNames.Contains("Calibiri"));

//            skin.window.padding.left = skin.window.padding.right = 3;
//            skin.scrollView.margin = new RectOffset(0, 0, 0, 0);// = new Vector2(20f, skin.scrollView.contentOffset.y);
//            skin.scrollView.clipping = TextClipping.Clip;

//            var toggleCheckedTexture = resources.GetTexture("Resources/toggleChecked.png");
//            var toggleUncheckedTexture = resources.GetTexture("Resources/toggleUnchecked.png");

//            if (!toggleCheckedTexture.Any() || !toggleUncheckedTexture.Any())
//                throw new FileNotFoundException("Missing custom toggle texture");

//            var litToggleChecked = toggleCheckedTexture.Single().Clone();
//            var litToggleUnchecked = toggleUncheckedTexture.Single().Clone();

//            litToggleChecked.ChangeLightness(1.1f);
//            litToggleUnchecked.ChangeLightness(1.1f);

//            skin.toggle.normal.background = skin.toggle.active.background = toggleUncheckedTexture.Single();
//            skin.toggle.onNormal.background = skin.toggle.onActive.background = toggleCheckedTexture.Single();

//            skin.toggle.focused.background = skin.toggle.hover.background = litToggleUnchecked;
//            skin.toggle.onFocused.background = skin.toggle.onHover.background = litToggleChecked;

//            //skin.horizontalScrollbar = new GUIStyle(skin.verticalScrollbar);

//            //Action<GUIStyleState, GUIStyleState, float, string> setState = (state, original, angle, tag) =>
//            //{
//            //    if (original.background.IsNull()) return;

//            //    state.background = original.background.CreateReadable().Rotate(angle);
//            //    state.background.As2D().SaveToDisk(tag + ".png");
//            //};

//            //skin.horizontalScrollbarLeftButton.normal.background =
//            //    skin.verticalScrollbarUpButton.normal.background.CreateReadable().Rotate(-90f);
//            //skin.horizontalScrollbarRightButton.normal.background =
//            //    skin.verticalScrollbarDownButton.normal.background.CreateReadable().Rotate(90f);

//            //setState(skin.horizontalScrollbarLeftButton.normal, skin.verticalScrollbarUpButton.normal, 90f, "leftButtonNormal");
//            //setState(skin.horizontalScrollbar.normal, skin.verticalScrollbar.normal, 90f, "scrollbarNormal");
//            //setState(skin.horizontalScrollbarThumb.normal, skin.verticalScrollbarThumb.normal, 0f, "scrollbarThumb");

//            //setState(skin.horizontalScrollbar.onNormal, skin.verticalScrollbar.onNormal, 90f, "scrollbarNormal");
//            //setState(skin.horizontalScrollbarThumb.onNormal, skin.verticalScrollbarThumb.onNormal, 0f, "scrollbarThumb");


//            //setState(skin.horizontalScrollbarLeftButton.normal, skin.verticalScrollbarUpButton.normal, -90f, "scrollbarLeft");

//            //skin.horizontalScrollbar.border = new RectOffset(4, 4, 2, 2);
//            //skin.horizontalScrollbar.clipping = TextClipping.Clip;
//            //skin.horizontalScrollbar.padding = new RectOffset(15, 15, 0, 0);
//            //skin.horizontalScrollbar.margin = new RectOffset(15, 15,0,0);
//            //skin.horizontalScrollbar.border = new RectOffset(15, 0, 0, 0);
//            //skin.horizontalScrollbar.contentOffset = new Vector2(20f, 0f);

//            //skin.horizontalScrollbarLeftButton.margin = new RectOffset(10, 10, 10, 10);
//            //skin.horizontalScrollbarLeftButton.padding = new RectOffset(10, 10, 10, 10);
//            //skin.horizontalScrollbarLeftButton.border = new RectOffset(10, 10, 10, 10);

            


//            //Action<GUIStyleState, string> save = (state, tag) =>
//            //{
//            //    if (state.background.IsNull()) return;

//            //    state.background.CreateReadable().SaveToDisk(tag + ".png");
//            //};

//            //save(skin.horizontalScrollbar.normal, "normal");
//            //save(skin.horizontalScrollbar.onNormal, "onNormal");
//            //save(skin.horizontalScrollbar.active, "active");
//            //save(skin.horizontalScrollbar.onActive, "onActive");
//            //save(skin.horizontalScrollbar.hover, "hover");
//            //save(skin.horizontalScrollbar.onHover, "onHover");
//            //save(skin.horizontalScrollbar.focused, "focused");
//            //save(skin.horizontalScrollbar.onFocused, "onFocused");
//            //save(skin.horizontalSlider.normal, "sliderNormal");

//            //skin.horizontalScrollbar.normal.background.As2D().ChangeLightness(2f);
//            //skin.horizontalSlider.normal.background = skin.horizontalSlider.normal.background.CreateReadable();
//            //skin.horizontalSlider.normal.background.ChangeLightness(0f);

//            //skin.verticalScrollbar.normal.background.As2D().CreateReadable().Rotate(45f).SaveToDisk("rotated45.png");
//            //skin.verticalScrollbar.normal.background.As2D().CreateReadable().Rotate(90f).SaveToDisk("rotated90.png");
//            //skin.verticalScrollbar.normal.background.As2D().CreateReadable().Rotate(0f).SaveToDisk("rotated0.png");
//            return skin;
//        }


//        private static GUISkin ConfigurePanelToggleStyle(IResourceRepository resources, GUISkin skin)
//        {
//            var customSkin = UnityEngine.Object.Instantiate(skin) as GUISkin;
//            if (customSkin == null) throw new NullReferenceException("Failed to clone skin");

//            var style = customSkin.toggle;
            
//            var panelExpandedTexture = resources.GetTexture("Resources/panelExpanded.png");
//            var panelCompactTexture = resources.GetTexture("Resources/panelCompact.png");

//            if (!panelExpandedTexture.Any() || !panelCompactTexture.Any())
//                throw new FileNotFoundException("Missing custom panel toggle texture");

//            panelExpandedTexture.Single().wrapMode = 
//                panelCompactTexture.Single().wrapMode = TextureWrapMode.Clamp;

//            panelCompactTexture.Single().ChangeLightness(0.6f);
//            panelExpandedTexture.Single().ChangeLightness(0.6f);

//            style.normal.background = style.active.background = panelCompactTexture.Single();
//            style.onNormal.background = style.onActive.background = panelExpandedTexture.Single();

//            var litExpanded = panelExpandedTexture.Single().Clone();
//            var litCompact = panelCompactTexture.Single().Clone();

//            litExpanded.ChangeLightness(1.1f);
//            litCompact.ChangeLightness(1.1f);

//            style.focused.background = style.hover.background = litCompact;
//            style.onFocused.background = style.onHover.background = litExpanded;

//            return customSkin;
//        }


//        private IAssemblyDefinitionWeaver ConfigureDefinitionWeaver(IFile location, PluginConfiguration pluginConfiguration)
//        {
//            if (location == null) throw new ArgumentNullException("location");
            
//            var getCodeBaseProperty = typeof (Assembly).GetProperty("CodeBase",
//                BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty);

//            var getLocationProperty = typeof (Assembly).GetProperty("Location",
//                BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty);
   
//            if (getCodeBaseProperty == null || getCodeBaseProperty.GetGetMethod() == null)
//                throw new MissingMethodException(typeof (Assembly).FullName, "CodeBase");

//            if (getLocationProperty == null || getCodeBaseProperty.GetGetMethod() == null)
//                throw new MissingMethodException(typeof (Assembly).FullName, "Location");


//            var uri = new Uri(location.FullPath);
//            var injectedHelperTypeQuery = new InjectedHelperGetTypeQuery();

//            var allTypesFromAssemblyExceptInjected = new GetTypeDefinitionsExcluding(
//                new GetAllTypesFromDefinition(), new InjectedHelperGetTypeQuery());
   
//            var renameAssembly = new RenameAssemblyOperation(new UniqueAssemblyNameGenerator(new RandomStringGenerator()));

//            var writeInjectedHelper = 
//                    new InjectedHelperTypeDefinitionWriter(
//                    new CompositeCommand<TypeDefinition>(
//                        new ProxyAssemblyMethodWriter(Uri.UnescapeDataString(uri.AbsoluteUri), getCodeBaseProperty.GetGetMethod()),
//                        new ProxyAssemblyMethodWriter(uri.LocalPath, getLocationProperty.GetGetMethod())));

//            var interceptAssemblyCodeBaseCalls = new InterceptExecutingAssemblyLocationQueries(
//                new MethodCallInMethodBodyQuery(
//                    getCodeBaseProperty.GetGetMethod(),
//                    OpCodes.Callvirt),
//                    new InjectedHelperTypeGetMethod(injectedHelperTypeQuery, getCodeBaseProperty.GetGetMethod().Name)
//                );

//            var interceptAssemblyLocationCalls = new InterceptExecutingAssemblyLocationQueries(
//                new MethodCallInMethodBodyQuery(
//                    getLocationProperty.GetGetMethod(),
//                    OpCodes.Callvirt),
//                new InjectedHelperTypeGetMethod(injectedHelperTypeQuery, getLocationProperty.GetGetMethod().Name)
//                );

//            return new AssemblyDefinitionWeaver(
//                _log.CreateTag("Weaver"), 
//                allTypesFromAssemblyExceptInjected,
//                new GetAllMethodDefinitions(),
//                renameAssembly,
//                new ConditionalWeaveOperation(writeInjectedHelper, () => pluginConfiguration.InjectHelperType),
//                new ConditionalWeaveOperation(interceptAssemblyCodeBaseCalls, () => pluginConfiguration.RewriteAssemblyLocationCalls),
//                new ConditionalWeaveOperation(interceptAssemblyLocationCalls, () => pluginConfiguration.RewriteAssemblyLocationCalls));

//        }


//        private IPartModuleSnapshotGenerator ConfigurePartModuleSnapshotGenerator(DictionaryQueue<KeyValuePair<uint, ITypeIdentifier>, ConfigNode> configNodeQueue)
//        {
//            if (configNodeQueue == null) throw new ArgumentNullException("configNodeQueue");

//            return new PartModuleSnapshotGenerator(
//                configNodeQueue,
//                new PartIsPrefabQuery(),
//                new TypeIdentifierQuery(),
//                new UniqueFlightIdGenerator(),
//                _log.CreateTag("PMSnapshotter"));
//        }


//        private GUIStyle ConfigureTitleBarButtonStyle()
//        {
//            var style = new GUIStyle(HighLogic.Skin.button) { border = new RectOffset(), padding = new RectOffset() };
//            style.fixedHeight = style.fixedWidth = 16f;
//            style.margin = new RectOffset();

//            return style;
//        }


//        private static Maybe<ConfigNode> GetWindowConfig(Maybe<ConfigNode> reloaderConfig, string nodeKey)
//        {
//            if (!reloaderConfig.Any() || !reloaderConfig.Single().HasNode(nodeKey)) return Maybe<ConfigNode>.None;

//            return Maybe<ConfigNode>.With(reloaderConfig.Single().GetNode(nodeKey));
//        }


//        private ICommand SetupSaveConfigurationCommand(
//            [NotNull] Configuration configuration, 
//            [NotNull] IConfigNodeSerializer serializer,
//            [NotNull] IFilePathProvider configPathProvider)
//        {
//            if (configuration == null) throw new ArgumentNullException("configuration");
//            if (serializer == null) throw new ArgumentNullException("serializer");
//            if (configPathProvider == null) throw new ArgumentNullException("configPathProvider");

//            var path = configPathProvider.Get();

//            return new Command(() =>
//            {
//                var node = new ConfigNode("AssemblyReloader");
//                serializer.Serialize(configuration, node);

//                if (!node.Save(path, "Assembly Reloader Configuration"))
//                    _log.Warning("Failed to save AssemblyReloader configuration to " + path);
//            });
//        }
//    }
//}
