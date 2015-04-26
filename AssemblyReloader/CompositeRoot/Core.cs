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
using Mono.Cecil;
using Mono.Cecil.Cil;
using ReeperCommon.FileSystem;
using ReeperCommon.FileSystem.Factories;
using ReeperCommon.FileSystem.Providers;
using ReeperCommon.Gui.Window.Providers;
using ReeperCommon.Logging;
using ReeperCommon.Repositories;
using ReeperCommon.Serialization;
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

            var configNodeFormatter = new ConfigNodeFormatter(
                new DefaultSurrogateSelector(new DefaultSurrogateProvider()),
                    new SerializableFieldQuery());

            var reloadables = CreateReloadablePlugins(loadedAssemblyFactory, fsFactory, assemblyResolver, new PluginConfigurationProvider(configNodeFormatter, new ConfigurationFilePathQuery())).ToList();

            reloadables.ForEach(r => r.Load());

            var skinScheme = ConfigureSkin();

            var windowFactory = new WindowFactory(
                resourceLocator, new ConfigurationPanelFactory(
                    new ExpandablePanelFactory(skinScheme.toggle, GUILayout.ExpandWidth(false),
                        GUILayout.ExpandHeight(false)),
                    new ConfigurationWindowPanelFieldQuery()),
                skinScheme,
                ConfigureTitleBarButtonStyle(),
                new SaveConfigurationCommand(configNodeFormatter));

            var uniqueIdProvider = new UniqueWindowIdProvider();


            var guiController =
                new GuiController(reloadables.ToDictionary(r => r,
                    r =>
                    {
                        var configWindow = windowFactory.CreatePluginOptionsWindow(r);

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
            BaseAssemblyResolver assemblyResolver,
            IPluginConfigurationProvider configurationProvider)
        {
            var reloadableAssemblyFileQuery = new ReloadableAssemblyFilesInDirectoryQuery(fsFactory.GetGameDataDirectory());


            return reloadableAssemblyFileQuery
                .Get()
                .Select(raFile => ConfigureReloadablePlugin(raFile, 
                    assemblyResolver, 
                    laFactory, 
                    configurationProvider));
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
            IPluginConfigurationProvider configurationProvider)
        {
            var configuration = configurationProvider.Get(location);

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
                ConfigureDefinitionWeaver(location, configuration));


            var kspFactory = new KspFactory(new KspGameObjectProvider());
            var reloadable = new ReloadablePlugin(new KspAssemblyLoader(assemblyProvider, laFactory), location, configuration);

            

            SetupAddonController(reloadable, configuration);
            SetupPartModuleController(reloadable, configuration, kspFactory);
            SetupScenarioModuleController(reloadable, configuration, kspFactory);


            

            return reloadable;
        }


        private static void SetupAddonController(IReloadablePlugin plugin, Configuration configuration)
        {
            var addonDestroyer = new AddonDestroyer(
                new UnityObjectDestroyer(new PluginReloadRequestedMethodCallCommand()),
                new LoadedComponentQuery(),
                new AddonsFromAssemblyQuery(new AddonAttributesFromTypeQuery()));

            var addonController =
                new AddonController(
                    new KspAddonLoader(),
                    addonDestroyer,
                    new CurrentStartupSceneProvider(new StartupSceneFromGameSceneQuery(),
                        new CurrentGameSceneProvider()));

            plugin.OnLoaded +=
                (asm, loc) => { if (configuration.StartAddonsForCurrentScene) addonController.Load(asm, loc); };

            plugin.OnUnloaded += addonController.Unload; 
        }


        private void SetupPartModuleController(IReloadablePlugin plugin, Configuration configuration, IKspFactory kspFactory)
        {
            var partModuleRepository = new FlightConfigRepository();


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

            plugin.OnLoaded += (asm, loc) =>
            {
                if (configuration.ReloadPartModulesImmediately) partModuleController.Load(asm, loc);
            };

            plugin.OnUnloaded += partModuleController.Unload;
        }


        private void SetupScenarioModuleController(IReloadablePlugin plugin, Configuration configuration, IKspFactory kspFactory)
        {
            var gameProvider = new CurrentGameProvider(new TypeIdentifierQuery());

            var protoScenarioModuleProvider = new ProtoScenarioModuleProvider(
                kspFactory,
                new TypeIdentifierQuery(),
                new CurrentGameProvider(new TypeIdentifierQuery()));

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

            plugin.OnLoaded += (asm, loc) =>
            {
                if (configuration.ReloadScenarioModulesImmediately) scenarioModuleController.Load(asm, loc);
            };

            plugin.OnUnloaded += scenarioModuleController.Unload;
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



        private IAssemblyDefinitionWeaver ConfigureDefinitionWeaver(IFile location, Configuration configuration)
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

            var writeInjectedHelper = 
                    new InjectedHelperTypeDefinitionWriter(
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
                new ConditionalWeaveOperation(writeInjectedHelper, () => configuration.InjectHelperType),
                new ConditionalWeaveOperation(replaceAssemblyLocationCalls, () => configuration.RewriteAssemblyLocationCalls));

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


        private GUIStyle ConfigureTitleBarButtonStyle()
        {
            var style = new GUIStyle(HighLogic.Skin.button) { border = new RectOffset(), padding = new RectOffset() };
            style.fixedHeight = style.fixedWidth = 16f;
            style.margin = new RectOffset();

            return style;
        }
    }
}
