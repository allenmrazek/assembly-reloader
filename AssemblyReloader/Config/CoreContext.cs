using System.Collections.Generic;
using System.Linq;
using AssemblyReloader.Common;
using AssemblyReloader.Game;
using AssemblyReloader.Gui;
using AssemblyReloader.ReloadablePlugin;
using AssemblyReloader.ReloadablePlugin.Config;
using AssemblyReloader.ReloadablePlugin.Loaders;
using AssemblyReloader.ReloadablePlugin.Weaving;
using AssemblyReloader.StrangeIoC.extensions.context.api;
using Mono.Cecil;
using ReeperCommon.FileSystem;
using ReeperCommon.Logging;
using UnityEngine;

namespace AssemblyReloader.Config
{
    public class CoreContext : ReeperCommonContext
    {
        public CoreContext(MonoBehaviour view)
            : base(view, ContextStartupFlags.AUTOMATIC)
        {
        }
        
        protected override void mapBindings()
        {
            base.mapBindings();

            



            // cross context bindings
            injectionBinder.Bind<GameObject>()
                .To(contextView as GameObject)
                .ToName(Keys.GameObjectKeys.CoreContext).CrossContext();

            injectionBinder.Bind<IGetRandomString>().To<GetRandomString>().ToSingleton().CrossContext();
            injectionBinder.Bind<IGetConfigurationFilePath>()
                .To(new GetConfigurationFilePath())
                .CrossContext();

            injectionBinder.Bind<Configuration>().ToSingleton().CrossContext();

            var assemblyResolver = new DefaultAssemblyResolver();
            assemblyResolver.AddSearchDirectory(injectionBinder.GetInstance<IDirectory>().FullPath);

            injectionBinder.Bind<BaseAssemblyResolver>().To(assemblyResolver).CrossContext();
            injectionBinder.Bind<IGetCurrentStartupScene>().To<GetCurrentStartupScene>().ToSingleton().CrossContext();
            
                // game events
                injectionBinder.Bind<SignalOnLevelWasLoaded>().ToSingleton().CrossContext();



            // bootstrap reloadable plugin contexts
            var pluginContexts =
                injectionBinder.GetInstance<GetReloadableAssemblyFilesFromDirectoryRecursive>().Get()
                    .Select(f =>
                    {
                        injectionBinder.GetInstance<ILog>().Normal("Bootstrapping context for " + f.Url);

                        var ctx = new ReloadablePluginContext(((GameObject) contextView).GetComponent<CoreBootstrapper>(), f);
                        ctx.Start();

                        return ctx;
                    })
                    .ToList();

            var pluginInfoMapping = pluginContexts.ToDictionary(context => context.Info, context => context.Plugin);


            // core context bindings

            injectionBinder.Bind<IEnumerable<ReloadablePluginContext>>().To(pluginContexts);
            injectionBinder.Bind<IEnumerable<IPluginInfo>>().To(pluginInfoMapping.Keys);
            injectionBinder.Bind<IEnumerable<IReloadablePlugin>>().To(pluginInfoMapping.Values);
            injectionBinder.Bind<IDictionary<IPluginInfo, IReloadablePlugin>>().To(pluginInfoMapping);

            mediationBinder.BindView<MainView>().ToMediator<MainViewMediator>();
            mediationBinder.BindView<ConfigurationView>().ToMediator<ConfigurationViewMediator>();
            mediationBinder.BindView<GameEventView>().ToMediator<GameEventMediator>();

            // set up command bindings
            commandBinder.Bind<SignalStart>()
                .InSequence()
                .To<CommandLoadConfiguration>()
                .To<CommandConfigureGameEvents>()
                .To<CommandLaunchReloadablePluginContexts>()
                .To<CommandConfigureGUI>()
                .Once();



            //var assemblyResolver = new DefaultAssemblyResolver();
            //assemblyResolver.AddSearchDirectory(injectionBinder.GetInstance<IDirectory>(DirectoryNames.Core).FullPath);

            //injectionBinder.Bind<BaseAssemblyResolver>().To(assemblyResolver).ToSingleton();

            //injectionBinder.Bind<Assembly>().To(Assembly.GetExecutingAssembly()).ToName(AssemblyNames.Core).ToSingleton();

            //injectionBinder.Bind<IUnityObjectDestroyer>()
            //    .ToValue(new UnityObjectDestroyer(new PluginReloadRequestedMethodCallCommand()));

            //injectionBinder.Bind<IResourceRepository>()
            //    .ToValue(ConfigureResourceRepository(injectionBinder.GetInstance<IDirectory>(DirectoryNames.Core)));

            //injectionBinder.Bind<WindowFactory>().To<WindowFactory>();
            //injectionBinder.Bind<GUIStyle>().To(ConfigureTitleBarButtonStyle()).ToName(Styles.TitleBarButtonStyle).ToSingleton();

            //BindTextureToName(injectionBinder.GetInstance<IResourceRepository>(), "Resources/btnClose", TextureNames.CloseButton);
            //BindTextureToName(injectionBinder.GetInstance<IResourceRepository>(), "Resources/btnWrench", TextureNames.SettingsButton);
            //BindTextureToName(injectionBinder.GetInstance<IResourceRepository>(), "Resources/cursor", TextureNames.ResizeCursor);


            //injectionBinder.Bind<IGetAttributesOfType<KSPAddon>>().To<GetAttributesOfType<KSPAddon>>().ToSingleton();
            //injectionBinder.Bind<IGetTypesFromAssembly<KSPAddonType>>().To<GetAddonTypesFromAssembly>().ToSingleton();
            //injectionBinder.Bind<IGetAssemblyFileLocation>().To<GetAssemblyFileLocation>().ToSingleton();

            //injectionBinder.Bind<IFile>()
            //    .ToValue(GetAssemblyFileLocation(Assembly.GetExecutingAssembly()))
            //    .ToName(FileNames.Core);


            //injectionBinder.Bind<IConfigurationPathProvider>()
            //    .To(
            //        new ConfigurationPathProvider(injectionBinder.GetInstance<IGetAssemblyFileLocation>()
            //            .Get(Assembly.GetExecutingAssembly())
            //            .Single(), "config"));





            //injectionBinder.Bind<IAssemblyProviderFactory>().To(new AssemblyProviderFactory(
            //    injectionBinder.GetInstance<BaseAssemblyResolver>(),
            //    injectionBinder.GetInstance<IGetDebugSymbolsExistForDefinition>(),
            //    injectionBinder.GetInstance<IWeaveOperationFactory>(),
            //    new GetTypeDefinitionsExcluding(new GetTypeDefinitionsInAssemblyDefinitionExcludingHelper(),
            //        new GetInjectedHelperTypeFromDefinition()),
            //    new GetMethodDefinitionsInTypeDefinition(),
            //    log.CreateTag("ILWeaver"),
            //    () => true)).ToSingleton();

            //var reloadableFileQuery =
            //    new GetReloadableAssemblyFilesFromDirectoryRecursive(
            //        injectionBinder.GetInstance<IDirectory>(DirectoryNames.GameData));

            //log.Normal("Reloadable files count: " + reloadableFileQuery.Get().Count());

            //var reloadablePluginFactory = injectionBinder.GetInstance<ReloadablePluginFactory>();

            //if (reloadablePluginFactory == null) throw new Exception("failed to create plugin factory");



            //var reloadablePlugins = reloadableFileQuery.Get().Select(file =>
            //{
            //    var plugin = reloadablePluginFactory.Create(file);

            //    plugin.Reload();

            //    return plugin;
            //}).ToList();

            //injectionBinder.Bind<IEnumerable<IPluginInfo>>().To(reloadablePlugins.Cast<IPluginInfo>()).ToSingleton();

            //log.Normal("Finished loading initial reloadable plugins.");





            //// create main window ...
            //mediationBinder.BindView<View>().ToMediator<MainViewMediator>();

        }


        public override void Launch()
        {
            base.Launch();
            injectionBinder.GetInstance<SignalStart>().Dispatch();
        }


        


        //private IAssemblyDefinitionWeaver ConfigureDefinitionWeaver([NotNull] IFile location,
        //    [NotNull] PluginConfiguration pluginConfiguration,
        //    [NotNull] ILog weaverLog)
        //{
        //    if (location == null) throw new ArgumentNullException("location");
        //    if (pluginConfiguration == null) throw new ArgumentNullException("pluginConfiguration");
        //    if (weaverLog == null) throw new ArgumentNullException("weaverLog");

        //    var getCodeBaseProperty = typeof(Assembly).GetProperty("CodeBase",
        //        BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty);

        //    var getLocationProperty = typeof(Assembly).GetProperty("Location",
        //        BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty);

        //    if (getCodeBaseProperty == null || getCodeBaseProperty.GetGetMethod() == null)
        //        throw new MissingMethodException(typeof(Assembly).FullName, "CodeBase");

        //    if (getLocationProperty == null || getCodeBaseProperty.GetGetMethod() == null)
        //        throw new MissingMethodException(typeof(Assembly).FullName, "Location");


        //    var uri = new Uri(location.FullPath);
        //    var injectedHelperTypeQuery = new GetInjectedHelperTypeFromDefinition();

        //    var allTypesFromAssemblyExceptInjected = new GetTypeDefinitionsExcluding(
        //        new GetTypeDefinitionsInAssemblyDefinitionExcludingHelper(), new GetInjectedHelperTypeFromDefinition());

        //    var renameAssembly = new RenameAssemblyOperation(new UniqueAssemblyNameGenerator(new RandomStringGenerator()));

        //    //var writeInjectedHelper =
        //    //        new InjectedHelperTypeDefinitionWriter(
        //    //        new CompositeCommand<TypeDefinition>(
        //    //            new ProxyAssemblyMethodWriter(Uri.UnescapeDataString(uri.AbsoluteUri), getCodeBaseProperty.GetGetMethod()),
        //    //            new ProxyAssemblyMethodWriter(uri.LocalPath, getLocationProperty.GetGetMethod())));

        //    var interceptAssemblyCodeBaseCalls = new InterceptExecutingAssemblyLocationQueries(
        //        new GetMethodCallsInMethod(
        //            getCodeBaseProperty.GetGetMethod(),
        //            OpCodes.Callvirt),
        //            new GetInjectedHelperTypeMethod(injectedHelperTypeQuery, getCodeBaseProperty.GetGetMethod().Name)
        //        );

        //    var interceptAssemblyLocationCalls = new InterceptExecutingAssemblyLocationQueries(
        //        new GetMethodCallsInMethod(
        //            getLocationProperty.GetGetMethod(),
        //            OpCodes.Callvirt),
        //        new GetInjectedHelperTypeMethod(injectedHelperTypeQuery, getLocationProperty.GetGetMethod().Name)
        //        );

        //    return new AssemblyDefinitionWeaver(
        //        weaverLog,
        //        allTypesFromAssemblyExceptInjected,
        //        new GetMethodDefinitionsInTypeDefinition(),
        //        renameAssembly,
        //        //new ConditionalWeaveOperation(writeInjectedHelper, () => pluginConfiguration.InjectHelperType),
        //        new ConditionalWeaveOperation(interceptAssemblyCodeBaseCalls, () => pluginConfiguration.RewriteAssemblyLocationCalls),
        //        new ConditionalWeaveOperation(interceptAssemblyLocationCalls, () => pluginConfiguration.RewriteAssemblyLocationCalls));

        //}


        





        
    }
}
