using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using AssemblyReloader.Commands;
using AssemblyReloader.Commands.old;
using AssemblyReloader.Common;
using AssemblyReloader.Config.Names;
using AssemblyReloader.FileSystem;
using AssemblyReloader.Gui;
using AssemblyReloader.Properties;
using AssemblyReloader.Queries;
using AssemblyReloader.Queries.CecilQueries;
using AssemblyReloader.ReloadablePlugin;
using AssemblyReloader.ReloadablePlugin.Config;
using AssemblyReloader.ReloadablePlugin.Definition;
using AssemblyReloader.ReloadablePlugin.Definition.Operations;
using AssemblyReloader.ReloadablePlugin.Definition.Operations.old;
using AssemblyReloader.StrangeIoC.extensions.context.api;
using AssemblyReloader.StrangeIoC.extensions.injector.api;
using AssemblyReloader.Weaving.old;
using Mono.Cecil;
using Mono.Cecil.Cil;
using ReeperCommon.FileSystem;
using ReeperCommon.Logging;
using ReeperCommon.Repositories;
using UnityEngine;
using AssemblyDefinitionWeaver = AssemblyReloader.Weaving.old.AssemblyDefinitionWeaver;

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
            injectionBinder.Bind<SignalStart>().ToSingleton().CrossContext();
            injectionBinder.Bind<IGetConfigurationFilePath>().To(new GetConfigurationFilePath()).CrossContext();
            injectionBinder.Bind<Configuration>().ToSingleton().CrossContext();
            injectionBinder.Bind<GameObject>()
                .To((contextView as GameObject) ?? new GameObject("CoreContext.CastFailed"))
                .ToName(Keys.GameObjectKeys.CoreContext).CrossContext();

            // gui and resource bindings
            injectionBinder.Bind<GUIStyle>().To(ConfigureTitleBarButtonStyle()).ToName(Styles.TitleBarButtonStyle).ToSingleton().CrossContext();
            BindTextureToName(injectionBinder.GetInstance<IResourceRepository>(), "Resources/btnClose", TextureNames.CloseButton).CrossContext();
            BindTextureToName(injectionBinder.GetInstance<IResourceRepository>(), "Resources/btnWrench", TextureNames.SettingsButton).CrossContext();
            BindTextureToName(injectionBinder.GetInstance<IResourceRepository>(), "Resources/cursor", TextureNames.ResizeCursor).CrossContext();


            // core context bindings
            injectionBinder.Bind<GetReloadableAssemblyFilesFromDirectoryRecursive>().ToSingleton();

            
            


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

            injectionBinder.Bind<IEnumerable<ReloadablePluginContext>>().To(pluginContexts);
            injectionBinder.Bind<IEnumerable<IPluginInfo>>().To(pluginInfoMapping.Keys);
            injectionBinder.Bind<IEnumerable<IReloadablePlugin>>().To(pluginInfoMapping.Values);
            injectionBinder.Bind<IDictionary<IPluginInfo, IReloadablePlugin>>().To(pluginInfoMapping);

            mediationBinder.BindView<MainView>().ToMediator<MainViewMediator>();
            mediationBinder.BindView<ConfigurationView>().ToMediator<ConfigurationViewMediator>();

            // set up command bindings
            commandBinder.Bind<SignalStart>()
                .To<CommandLoadConfiguration>()
                .To<CommandLaunchReloadablePluginContexts>()
                .To<CommandConfigureGUI>()
                .Once()
                .InSequence();


            //(contextView as GameObject ?? new GameObject("ConfigurationView")).AddComponent<ConfigurationView>();
            //(contextView as GameObject ?? new GameObject("CoreContext")).AddComponent<MainView>();


            //serializerSelector.AddSerializer(typeof (Setting<>), SettingSerializerFactory.Create);



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
            //    new GetTypeDefinitionsExcluding(new GetAllTypesFromDefinition(),
            //        new GetInjectedHelperTypeFromDefinition()),
            //    new GetAllMethodDefinitions(),
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


        


        private IAssemblyDefinitionWeaver ConfigureDefinitionWeaver([NotNull] IFile location,
            [NotNull] PluginConfiguration pluginConfiguration,
            [NotNull] ILog weaverLog)
        {
            if (location == null) throw new ArgumentNullException("location");
            if (pluginConfiguration == null) throw new ArgumentNullException("pluginConfiguration");
            if (weaverLog == null) throw new ArgumentNullException("weaverLog");

            var getCodeBaseProperty = typeof(Assembly).GetProperty("CodeBase",
                BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty);

            var getLocationProperty = typeof(Assembly).GetProperty("Location",
                BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty);

            if (getCodeBaseProperty == null || getCodeBaseProperty.GetGetMethod() == null)
                throw new MissingMethodException(typeof(Assembly).FullName, "CodeBase");

            if (getLocationProperty == null || getCodeBaseProperty.GetGetMethod() == null)
                throw new MissingMethodException(typeof(Assembly).FullName, "Location");


            var uri = new Uri(location.FullPath);
            var injectedHelperTypeQuery = new GetInjectedHelperTypeFromDefinition();

            var allTypesFromAssemblyExceptInjected = new GetTypeDefinitionsExcluding(
                new GetAllTypesFromDefinition(), new GetInjectedHelperTypeFromDefinition());

            var renameAssembly = new RenameAssemblyOperation(new UniqueAssemblyNameGenerator(new RandomStringGenerator()));

            //var writeInjectedHelper =
            //        new InjectedHelperTypeDefinitionWriter(
            //        new CompositeCommand<TypeDefinition>(
            //            new ProxyAssemblyMethodWriter(Uri.UnescapeDataString(uri.AbsoluteUri), getCodeBaseProperty.GetGetMethod()),
            //            new ProxyAssemblyMethodWriter(uri.LocalPath, getLocationProperty.GetGetMethod())));

            var interceptAssemblyCodeBaseCalls = new InterceptExecutingAssemblyLocationQueries(
                new MethodCallInMethodBodyQuery(
                    getCodeBaseProperty.GetGetMethod(),
                    OpCodes.Callvirt),
                    new GetInjectedHelperTypeMethod(injectedHelperTypeQuery, getCodeBaseProperty.GetGetMethod().Name)
                );

            var interceptAssemblyLocationCalls = new InterceptExecutingAssemblyLocationQueries(
                new MethodCallInMethodBodyQuery(
                    getLocationProperty.GetGetMethod(),
                    OpCodes.Callvirt),
                new GetInjectedHelperTypeMethod(injectedHelperTypeQuery, getLocationProperty.GetGetMethod().Name)
                );

            return new AssemblyDefinitionWeaver(
                weaverLog,
                allTypesFromAssemblyExceptInjected,
                new GetAllMethodDefinitions(),
                renameAssembly,
                //new ConditionalWeaveOperation(writeInjectedHelper, () => pluginConfiguration.InjectHelperType),
                new ConditionalWeaveOperation(interceptAssemblyCodeBaseCalls, () => pluginConfiguration.RewriteAssemblyLocationCalls),
                new ConditionalWeaveOperation(interceptAssemblyLocationCalls, () => pluginConfiguration.RewriteAssemblyLocationCalls));

        }


        private static GUIStyle ConfigureTitleBarButtonStyle()
        {
            var style = new GUIStyle(HighLogic.Skin.button) { border = new RectOffset(), padding = new RectOffset() };
            style.fixedHeight = style.fixedWidth = 16f;
            style.margin = new RectOffset();

            return style;
        }


        private IInjectionBinding BindTextureToName(IResourceRepository resourceRepo, string url, object name)
        {
            if (resourceRepo == null) throw new ArgumentNullException("resourceRepo");
            if (name == null) throw new ArgumentNullException("name");
            if (string.IsNullOrEmpty(url)) throw new ArgumentException("url is null or empty");

            var tex = resourceRepo.GetTexture(url);

            if (!tex.Any())
                throw new Exception("Couldn't bind texture at \"" + url + "\"; texture not found");

            return injectionBinder.Bind<Texture2D>().ToValue(tex.Single()).ToName(name);
        }


        private IFile GetAssemblyFileLocation(Assembly target)
        {
            if (target == null) throw new ArgumentNullException("target");

            var mf = injectionBinder.GetInstance<IGetAssemblyFileLocation>().Get(target);

            if (!mf.Any()) throw new Exception("Failed to find file location of " + target.FullName);
            return mf.Single();
        }



    }
}
