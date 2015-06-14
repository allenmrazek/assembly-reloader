using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using AssemblyReloader.Annotations;
using AssemblyReloader.Commands;
using AssemblyReloader.Controllers;
using AssemblyReloader.DataObjects;
using AssemblyReloader.Game;
using AssemblyReloader.Game.Providers;
using AssemblyReloader.Game.Queries;
using AssemblyReloader.Generators;
using AssemblyReloader.Gui;
using AssemblyReloader.Gui.Messages;
using AssemblyReloader.Loaders;
using AssemblyReloader.Loaders.PartModuleLoader;
using AssemblyReloader.Loaders.ScenarioModuleLoader;
using AssemblyReloader.Providers;
using AssemblyReloader.Queries;
using AssemblyReloader.Queries.AssemblyQueries;
using AssemblyReloader.Queries.CecilQueries;
using AssemblyReloader.Queries.FileSystemQueries;
using AssemblyReloader.TypeInstallers;
using AssemblyReloader.Weaving;
using AssemblyReloader.Weaving.Operations;
using Mono.Cecil;
using Mono.Cecil.Cil;
using ReeperCommon.Containers;
using ReeperCommon.Extensions;
using ReeperCommon.FileSystem;
using ReeperCommon.FileSystem.Factories;
using ReeperCommon.FileSystem.Providers;
using ReeperCommon.Gui;
using ReeperCommon.Logging;
using ReeperCommon.Repositories;
using ReeperCommon.Serialization;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AssemblyReloader.CompositeRoot
{
    // composite root
    class Core
    {
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

            _log = primaryLog;

            var fsFactory = new KSPFileSystemFactory(
                new KSPUrlDir(new KSPGameDataUrlDirProvider().Get()));

            var configFormatter = new ConfigNodeFormatter(new DefaultSurrogateSelector(new DefaultSurrogateProvider()),
                new CompositeFieldInfoQuery(new RecursiveSerializableFieldQuery()));

            var assemblyLoader = new KspAssemblyLoader();

            var assemblyFileProvider = new AssemblyFileLocationQuery(assemblyLoader, fsFactory);
            var mainAssemblyFile = assemblyFileProvider.Get(Assembly.GetExecutingAssembly());
            if (!mainAssemblyFile.Any()) throw new Exception("Failed to locate executing assembly file");

            var ourDirProvider = new AssemblyDirectoryQuery(assemblyLoader, Assembly.GetExecutingAssembly(), fsFactory.GetGameDataDirectory());

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

            var pluginConfigurationPathQuery = new PluginConfigurationFilePathQuery();

            var reloadables = CreateReloadablePlugins(loadedAssemblyFactory, fsFactory, assemblyResolver, new PluginConfigurationProvider(pluginConfigurationPathQuery)).ToList();

            reloadables.ForEach(r => r.Load());

            var skinScheme = ConfigureSkin(resourceLocator);

            var btnCloseTexture = resourceLocator.GetTexture("Resources/btnClose.png");
            if (!btnCloseTexture.Any()) throw new Exception("Failed to find window close button texture!");

            var btnOptionsTexture = resourceLocator.GetTexture("Resources/btnWrench.png");
            if (!btnOptionsTexture.Any()) throw new Exception("Failed to find window option button texture!");

            var btnResizeCursorTexture = resourceLocator.GetTexture("Resources/cursor.png");
            if (!btnResizeCursorTexture.Any()) throw new Exception("Failed to find window resize cursor texture!");

            var configFilePathQuery = new PluginConfigurationFilePathQuery();

            var configurationProvider = new ConfigurationProvider(
                mainAssemblyFile.Single(),
                configFilePathQuery,
                _log.CreateTag("ConfigurationProvider"));

            var configuration = configurationProvider.Get();

            //var saveProgramConfiguration = new SaveConfigurationCommand(configuration,
            //    new ConfigFilePathQuery(mainAssemblyFile.Single()), _log.CreateTag("Configuration"));

            var mainConfigFilePathQuery = new ConfigFilePathQuery(mainAssemblyFile.Single());
 
            var saveProgramConfigurationCallbacks = new SaveConfigNodeFromCallbacksCommand("Configuration",
                mainConfigFilePathQuery, "Assembly Reloader Configuration");

            var savePluginConfiguration = new SavePluginConfigurationCommand(configFilePathQuery);

            var mediator =
                new Controllers.Controller(
                    reloadables.ToDictionary(r => r as IPluginInfo, r => r as IReloadablePlugin),
                    saveProgramConfigurationCallbacks,
                    savePluginConfiguration,
                    _log.CreateTag("Controller"));

            var viewMessageChannel = new MessageChannel();

            var windowFactory = new WindowFactory(new UniqueWindowIdProvider(), mediator, viewMessageChannel,
                ConfigureTitleBarButtonStyle(), btnOptionsTexture.Single(), btnCloseTexture.Single());

            var mainAppearance = new WindowAppearanceInfo(skinScheme,
                new Rect(200f, 200f, 400f, 200f), new Vector2(10f, 10f), new Vector2(150f, 100f),
                btnResizeCursorTexture.Single());

            var windowDescriptors = new List<WindowDescriptor>();

            try
            {
                var mainWindow = windowFactory.CreateMainWindow(reloadables.Cast<IPluginInfo>(), mainAppearance,
                    Maybe<ConfigNode>.None);
                windowDescriptors.Add(mainWindow);

                saveProgramConfigurationCallbacks.OnExecute += node =>
                {
                    _log.Warning("Saving from type " + mainWindow.DecoratedLogic.GetType().FullName);
                    mainWindow.DecoratedLogic.Save(configFormatter, node.AddNode("MainWindow")); 
                };
                saveProgramConfigurationCallbacks.OnExecute += node => ConfigNode.CreateConfigFromObject(configuration, node);

                var optionsWindow = windowFactory.CreateOptionsWindow(mainAppearance, configuration, Maybe<ConfigNode>.None);
                windowDescriptors.Add(optionsWindow);
                viewMessageChannel.AddListener<ShowConfigurationWindow>(optionsWindow.BaseLogic);

                reloadables.ForEach(r =>
                {
                    var pluginConfigWindow = windowFactory.CreatePluginOptionsWindow(mainAppearance, r as IPluginInfo,
                        Maybe<ConfigNode>.None);
                    windowDescriptors.Add(pluginConfigWindow);

                    viewMessageChannel.AddListener<ShowPluginConfigurationWindow>(pluginConfigWindow.BaseLogic);
                });

                
            }
            catch (Exception)
            {
                _log.Error("Encountered an exception while creating windows");

                // need to destroy the windows, otherwise they'll stick around on unhandled exceptions
                windowDescriptors.ForEach(d => Object.Destroy(d.View));

                throw;
            }
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


        private ILoadedAssemblyFactory ConfigureLoadedAssemblyFactory()
        {
            return new KspLoadedAssemblyFactory(
                new LoadedAssemblyFileUrlQuery(),
                new DisposeLoadedAssemblyCommandFactory(),
                // note: no KSPAddon type installer required; game looks through LoadedAssemblies on every scene checking all types
                new GenericTypeInstaller<Part>(new TypesDerivedFromQuery<Part>()),
                new GenericTypeInstaller<PartModule>(new TypesDerivedFromQuery<PartModule>()),
                new GenericTypeInstaller<ScenarioModule>(new TypesDerivedFromQuery<ScenarioModule>()));
        }


        private IEnumerable<ReloadablePlugin> CreateReloadablePlugins(
            ILoadedAssemblyFactory laFactory,
            IFileSystemFactory fsFactory, 
            BaseAssemblyResolver assemblyResolver,
            IPluginConfigurationProvider configurationProvider)
        {
            var reloadableAssemblyFileQuery = new ReloadableAssemblyFilesInDirectoryQuery(fsFactory.GetGameDataDirectory());
            var unityObjectDestroyer = new UnityObjectDestroyer(new PluginReloadRequestedMethodCallCommand());
            var gameAssemblyLoader = new KspAssemblyLoader();
            var gameAddonLoader = new KspAddonLoader();

            return reloadableAssemblyFileQuery
                .Get()
                .Select(raFile => ConfigureReloadablePlugin(raFile,
                    gameAssemblyLoader,
                    gameAddonLoader,
                    assemblyResolver, 
                    laFactory, 
                    configurationProvider,
                    unityObjectDestroyer));
        }


        private static IEventProvider ConfigureEventProvider(IStartupSceneFromGameSceneQuery query)
        {
            if (query == null) throw new ArgumentNullException("query");

            var onLevelWasLoaded = new GameEventPublisher<GameScenes>();
            GameEvents.onLevelWasLoaded.Add(onLevelWasLoaded.Raise);

            var onSceneLoaded = new GameEventPublisher<KSPAddon.Startup>();
            onLevelWasLoaded.OnEvent += gameScene => onSceneLoaded.Raise(query.Get(gameScene));

            return new EventProvider{OnLevelWasLoaded = onLevelWasLoaded,
                OnSceneLoaded = onSceneLoaded};
        }


        private ReloadablePlugin ConfigureReloadablePlugin(
            IFile location,
            IGameAssemblyLoader gameAssemblyLoader,
            IGameAddonLoader gameAddonLoader,
            BaseAssemblyResolver assemblyResolver,
            ILoadedAssemblyFactory laFactory,
            IPluginConfigurationProvider configurationProvider,
            IUnityObjectDestroyer objectDestroyer)
        {
            var configuration = configurationProvider.Get(location);

            var debugSymbolExistQuery = new DebugSymbolFileExistsQuery(location);

            var assemblyProvider = new AssemblyProvider(
                new AssemblyDefinitionFromDiskReader(
                    location,
                    debugSymbolExistQuery,
                    assemblyResolver),
                new ConditionalWriteLoadedAssemblyToDisk(
                    new AssemblyDefinitionLoader(
                        new TemporaryFileFactory(
                            location.Directory,
                            new RandomStringGenerator()),
                        _log.CreateTag("AssemblyDefinitionLoader")),
                    () => configuration.WriteReweavedAssemblyToDisk,
                    location.Directory),
                ConfigureDefinitionWeaver(location, configuration));


            var kspFactory = new KspFactory(new KspGameObjectProvider());
            var reloadable = new ReloadablePlugin(new Loaders.AssemblyLoader(assemblyProvider, laFactory, _log.CreateTag("AssemblyLoader")), location, configuration);

            SetupAddonController(reloadable, gameAssemblyLoader, gameAddonLoader, objectDestroyer);
            SetupPartModuleController(reloadable, kspFactory);
            SetupScenarioModuleController(reloadable, configuration, kspFactory);

            return reloadable;
        }


        private static void SetupAddonController(
            [NotNull] ReloadablePlugin plugin, 
            [NotNull] IGameAssemblyLoader gameAssemblyLoader,
            [NotNull] IGameAddonLoader gameAddonLoader,
            [NotNull] IUnityObjectDestroyer objectDestroyer)

        {
            if (plugin == null) throw new ArgumentNullException("plugin");
            if (gameAssemblyLoader == null) throw new ArgumentNullException("gameAssemblyLoader");
            if (gameAddonLoader == null) throw new ArgumentNullException("gameAddonLoader");
            if (objectDestroyer == null) throw new ArgumentNullException("objectDestroyer");

            var addonLoader = new Loaders.AddonLoader(
                gameAssemblyLoader,
                gameAddonLoader,
                new CurrentStartupSceneProvider(new StartupSceneFromGameSceneQuery(), new CurrentGameSceneProvider()),
                () => plugin.Configuration.InstantlyAppliesToEveryScene);

            var addonUnloader = new AddonUnloader(
                new AddonsFromAssemblyQuery(new AddonAttributesFromTypeQuery()),
                objectDestroyer,
                new LoadedComponentQuery());

            var addonController = new AddonController(addonLoader, addonUnloader);

            plugin.OnLoaded +=
                (asm, loc) => { if (plugin.Configuration.StartAddonsForCurrentScene) addonController.Load(asm, loc); };

            plugin.OnUnloaded += addonController.Unload; 
        }


        private void SetupPartModuleController(ReloadablePlugin plugin, IKspFactory kspFactory)
        {
            var partModuleConfigQueue = new DictionaryQueue<KeyValuePair<uint, ITypeIdentifier>, ConfigNode>(
                new FlightConfigNodeKeyValuePairComparer()); 
            

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

            Func<bool> reuseConfigNodes = () => plugin.Configuration.ReusePartModuleConfigsFromPrevious;

            var onStartRunner = new ExecutePartModuleOnStartsCommand(new PartModuleStartStateProvider(),
                new PartIsPrefabQuery(), kspFactory, _log.CreateTag(plugin.Name + " OnStart Runner"));

            var partModuleController = new PartModuleController(
                                         new PartModuleLoader(
                                             descriptorFactory,
                                             new PartModuleFactory(new PartIsPrefabQuery(), new AwakenPartModuleCommand(), onStartRunner),
                                             partModuleConfigQueue,
                                             prefabCloneProvider,
                                             reuseConfigNodes),
                                         new PartModuleUnloader(
                                             new UnityObjectDestroyer(new PluginReloadRequestedMethodCallCommand()),
                                             descriptorFactory,
                                             prefabCloneProvider,
                                             ConfigurePartModuleSnapshotGenerator(partModuleConfigQueue),
                                             reuseConfigNodes
                                             ),
                                         new TypesDerivedFromQuery<PartModule>(),
                                         new CompositeCommand(
                                            onStartRunner,
                                            new ClearDictionaryQueryCommand<KeyValuePair<uint, ITypeIdentifier>, ConfigNode>(partModuleConfigQueue), 
                                            new RefreshPartActionWindows(KspPartActionWindowListener.WindowController)),
                                            new NullCommand());

            plugin.OnLoaded += (asm, loc) =>
            {
                onStartRunner.Clear();
                if (plugin.Configuration.ReloadPartModulesImmediately) partModuleController.Load(asm, loc);
            };

            plugin.OnUnloaded += partModuleController.Unload;
        }


        private void SetupScenarioModuleController(IReloadablePlugin plugin, PluginConfiguration pluginConfiguration, IKspFactory kspFactory)
        {
            var gameProvider = new CurrentGameProvider(new TypeIdentifierQuery());
            var currentGameSceneProvider = new CurrentGameSceneProvider();

            var protoScenarioModuleProvider = new ProtoScenarioModuleProvider(
                kspFactory,
                new TypeIdentifierQuery(),
                new CurrentGameProvider(new TypeIdentifierQuery()),
                currentGameSceneProvider);

            var scenarioModuleController =
                new ScenarioModuleController(
                    new ScenarioModuleLoader(protoScenarioModuleProvider),
                    new ScenarioModuleUnloader(
                        new GameObjectComponentQuery(new KspGameObjectProvider()),
                        protoScenarioModuleProvider,
                        new UnityObjectDestroyer(new PluginReloadRequestedMethodCallCommand()),
                        () => pluginConfiguration.SaveScenarioModuleConfigBeforeReloading,
                        new ScenarioModuleSnapshotGenerator(gameProvider, _log.CreateTag("SMSnapshotGen")),
                        _log.CreateTag("ScenarioModuleUnloader")),
                    new TypesDerivedFromQuery<ScenarioModule>(),
                    currentGameSceneProvider);

            plugin.OnLoaded += (asm, loc) =>
            {
                if (pluginConfiguration.ReloadScenarioModulesImmediately) scenarioModuleController.Load(asm, loc);
            };

            plugin.OnUnloaded += scenarioModuleController.Unload;
        }


        private GUISkin ConfigureSkin(IResourceRepository resources)
        {
            Resources.FindObjectsOfTypeAll<Font>()
                .ToList()
                .ForEach(
                    f =>
                        new DebugLog().Normal("ConfigureSkin: font = " +
                                              f.fontNames.Aggregate(string.Empty, (s1, s2) => s1 + ", " + s2)));

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
            if (skin == null) throw new Exception("Failed to clone OrbitMapSkin");

            skin.font = Resources.FindObjectsOfTypeAll<Font>().FirstOrDefault(f => f.fontNames.Contains("Calibiri"));

            skin.window.padding.left = skin.window.padding.right = 3;
            skin.scrollView.margin = new RectOffset(0, 0, 0, 0);// = new Vector2(20f, skin.scrollView.contentOffset.y);
            skin.scrollView.clipping = TextClipping.Clip;

            var toggleCheckedTexture = resources.GetTexture("Resources/toggleChecked.png");
            var toggleUncheckedTexture = resources.GetTexture("Resources/toggleUnchecked.png");

            if (!toggleCheckedTexture.Any() || !toggleUncheckedTexture.Any())
                throw new FileNotFoundException("Missing custom toggle texture");

            var litToggleChecked = toggleCheckedTexture.Single().Clone();
            var litToggleUnchecked = toggleUncheckedTexture.Single().Clone();

            litToggleChecked.ChangeLightness(1.1f);
            litToggleUnchecked.ChangeLightness(1.1f);

            skin.toggle.normal.background = skin.toggle.active.background = toggleUncheckedTexture.Single();
            skin.toggle.onNormal.background = skin.toggle.onActive.background = toggleCheckedTexture.Single();

            skin.toggle.focused.background = skin.toggle.hover.background = litToggleUnchecked;
            skin.toggle.onFocused.background = skin.toggle.onHover.background = litToggleChecked;

            //skin.horizontalScrollbar = new GUIStyle(skin.verticalScrollbar);

            //Action<GUIStyleState, GUIStyleState, float, string> setState = (state, original, angle, tag) =>
            //{
            //    if (original.background.IsNull()) return;

            //    state.background = original.background.CreateReadable().Rotate(angle);
            //    state.background.As2D().SaveToDisk(tag + ".png");
            //};

            //skin.horizontalScrollbarLeftButton.normal.background =
            //    skin.verticalScrollbarUpButton.normal.background.CreateReadable().Rotate(-90f);
            //skin.horizontalScrollbarRightButton.normal.background =
            //    skin.verticalScrollbarDownButton.normal.background.CreateReadable().Rotate(90f);

            //setState(skin.horizontalScrollbarLeftButton.normal, skin.verticalScrollbarUpButton.normal, 90f, "leftButtonNormal");
            //setState(skin.horizontalScrollbar.normal, skin.verticalScrollbar.normal, 90f, "scrollbarNormal");
            //setState(skin.horizontalScrollbarThumb.normal, skin.verticalScrollbarThumb.normal, 0f, "scrollbarThumb");

            //setState(skin.horizontalScrollbar.onNormal, skin.verticalScrollbar.onNormal, 90f, "scrollbarNormal");
            //setState(skin.horizontalScrollbarThumb.onNormal, skin.verticalScrollbarThumb.onNormal, 0f, "scrollbarThumb");


            //setState(skin.horizontalScrollbarLeftButton.normal, skin.verticalScrollbarUpButton.normal, -90f, "scrollbarLeft");

            //skin.horizontalScrollbar.border = new RectOffset(4, 4, 2, 2);
            //skin.horizontalScrollbar.clipping = TextClipping.Clip;
            //skin.horizontalScrollbar.padding = new RectOffset(15, 15, 0, 0);
            //skin.horizontalScrollbar.margin = new RectOffset(15, 15,0,0);
            //skin.horizontalScrollbar.border = new RectOffset(15, 0, 0, 0);
            //skin.horizontalScrollbar.contentOffset = new Vector2(20f, 0f);

            //skin.horizontalScrollbarLeftButton.margin = new RectOffset(10, 10, 10, 10);
            //skin.horizontalScrollbarLeftButton.padding = new RectOffset(10, 10, 10, 10);
            //skin.horizontalScrollbarLeftButton.border = new RectOffset(10, 10, 10, 10);

            


            //Action<GUIStyleState, string> save = (state, tag) =>
            //{
            //    if (state.background.IsNull()) return;

            //    state.background.CreateReadable().SaveToDisk(tag + ".png");
            //};

            //save(skin.horizontalScrollbar.normal, "normal");
            //save(skin.horizontalScrollbar.onNormal, "onNormal");
            //save(skin.horizontalScrollbar.active, "active");
            //save(skin.horizontalScrollbar.onActive, "onActive");
            //save(skin.horizontalScrollbar.hover, "hover");
            //save(skin.horizontalScrollbar.onHover, "onHover");
            //save(skin.horizontalScrollbar.focused, "focused");
            //save(skin.horizontalScrollbar.onFocused, "onFocused");
            //save(skin.horizontalSlider.normal, "sliderNormal");

            //skin.horizontalScrollbar.normal.background.As2D().ChangeLightness(2f);
            //skin.horizontalSlider.normal.background = skin.horizontalSlider.normal.background.CreateReadable();
            //skin.horizontalSlider.normal.background.ChangeLightness(0f);

            //skin.verticalScrollbar.normal.background.As2D().CreateReadable().Rotate(45f).SaveToDisk("rotated45.png");
            //skin.verticalScrollbar.normal.background.As2D().CreateReadable().Rotate(90f).SaveToDisk("rotated90.png");
            //skin.verticalScrollbar.normal.background.As2D().CreateReadable().Rotate(0f).SaveToDisk("rotated0.png");
            return skin;
        }


        private static GUISkin ConfigurePanelToggleStyle(IResourceRepository resources, GUISkin skin)
        {
            var customSkin = UnityEngine.Object.Instantiate(skin) as GUISkin;
            if (customSkin == null) throw new NullReferenceException("Failed to clone skin");

            var style = customSkin.toggle;
            
            var panelExpandedTexture = resources.GetTexture("Resources/panelExpanded.png");
            var panelCompactTexture = resources.GetTexture("Resources/panelCompact.png");

            if (!panelExpandedTexture.Any() || !panelCompactTexture.Any())
                throw new FileNotFoundException("Missing custom panel toggle texture");

            panelExpandedTexture.Single().wrapMode = 
                panelCompactTexture.Single().wrapMode = TextureWrapMode.Clamp;

            panelCompactTexture.Single().ChangeLightness(0.6f);
            panelExpandedTexture.Single().ChangeLightness(0.6f);

            style.normal.background = style.active.background = panelCompactTexture.Single();
            style.onNormal.background = style.onActive.background = panelExpandedTexture.Single();

            var litExpanded = panelExpandedTexture.Single().Clone();
            var litCompact = panelCompactTexture.Single().Clone();

            litExpanded.ChangeLightness(1.1f);
            litCompact.ChangeLightness(1.1f);

            style.focused.background = style.hover.background = litCompact;
            style.onFocused.background = style.onHover.background = litExpanded;

            return customSkin;
        }


        private IAssemblyDefinitionWeaver ConfigureDefinitionWeaver(IFile location, PluginConfiguration pluginConfiguration)
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
            var injectedHelperTypeQuery = new InjectedHelperTypeQuery();

            var allTypesFromAssemblyExceptInjected = new ExcludingTypeDefinitions(
                new AllTypesFromDefinitionQuery(), new InjectedHelperTypeQuery());
   
            var renameAssembly = new RenameAssemblyOperation(new UniqueAssemblyNameGenerator(new RandomStringGenerator()));

            var writeInjectedHelper = 
                    new InjectedHelperTypeDefinitionWriter(
                    new CompositeCommand<TypeDefinition>(
                        new ProxyAssemblyMethodWriter(Uri.UnescapeDataString(uri.AbsoluteUri), getCodeBaseProperty.GetGetMethod()),
                        new ProxyAssemblyMethodWriter(uri.LocalPath, getLocationProperty.GetGetMethod())));

            var interceptAssemblyCodeBaseCalls = new InterceptExecutingAssemblyLocationQueries(
                new MethodCallInMethodBodyQuery(
                    getCodeBaseProperty.GetGetMethod(),
                    OpCodes.Callvirt),
                    new InjectedHelperTypeMethodQuery(injectedHelperTypeQuery, getCodeBaseProperty.GetGetMethod().Name)
                );

            var interceptAssemblyLocationCalls = new InterceptExecutingAssemblyLocationQueries(
                new MethodCallInMethodBodyQuery(
                    getLocationProperty.GetGetMethod(),
                    OpCodes.Callvirt),
                new InjectedHelperTypeMethodQuery(injectedHelperTypeQuery, getLocationProperty.GetGetMethod().Name)
                );

            return new AssemblyDefinitionWeaver(
                _log.CreateTag("Weaver"), 
                allTypesFromAssemblyExceptInjected,
                new AllMethodsFromDefinitionQuery(),
                renameAssembly,
                new ConditionalWeaveOperation(writeInjectedHelper, () => pluginConfiguration.InjectHelperType),
                new ConditionalWeaveOperation(interceptAssemblyCodeBaseCalls, () => pluginConfiguration.RewriteAssemblyLocationCalls),
                new ConditionalWeaveOperation(interceptAssemblyLocationCalls, () => pluginConfiguration.RewriteAssemblyLocationCalls));

        }


        private IPartModuleSnapshotGenerator ConfigurePartModuleSnapshotGenerator(DictionaryQueue<KeyValuePair<uint, ITypeIdentifier>, ConfigNode> configNodeQueue)
        {
            if (configNodeQueue == null) throw new ArgumentNullException("configNodeQueue");

            return new PartModuleSnapshotGenerator(
                configNodeQueue,
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
