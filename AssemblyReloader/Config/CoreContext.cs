using System.Collections.Generic;
using System.Linq;
using AssemblyReloader.Config.Keys;
using AssemblyReloader.Game;
using AssemblyReloader.Gui;
using AssemblyReloader.ReloadablePlugin;
using AssemblyReloader.ReloadablePlugin.Config;
using AssemblyReloader.ReloadablePlugin.Loaders;
using AssemblyReloader.ReloadablePlugin.Loaders.Addons;
using AssemblyReloader.ReloadablePlugin.Loaders.PartModules;
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
            : base(view, ContextStartupFlags.MANUAL_MAPPING | ContextStartupFlags.MANUAL_LAUNCH)
        {
        }
        
        protected override void mapBindings()
        {
            base.mapBindings();

            MapCrossContextBindings(); // these bindings will be shared by the reloadable plugin contexts we're about to create

            // bootstrap reloadable plugin contexts
            var pluginContexts =
                injectionBinder.GetInstance<GetReloadableAssemblyFilesFromDirectoryRecursive>().Get()
                    .Select(reloadableFile =>
                    {
                        injectionBinder.GetInstance<ILog>().Normal("Bootstrapping context for " + reloadableFile.Url);

                        var reloadablePluginBootstrap = new GameObject("ContextView." + reloadableFile.Name);
                        reloadablePluginBootstrap.transform.parent = ((GameObject)contextView).transform;
                        Object.DontDestroyOnLoad(reloadablePluginBootstrap);

                        var pluginContextView = reloadablePluginBootstrap.AddComponent<BootstrapReloadablePlugin>();

                        pluginContextView.Bootstrap(reloadableFile);

                        return pluginContextView.context as ReloadablePluginContext;
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
                .To<CommandConfigureGui>()
                .To<CommandLaunchReloadablePluginContexts>()
                .Once();
        }


        private void MapCrossContextBindings()
        {
            injectionBinder.Bind<IGetConfigurationFilePath>()
                .To(new GetConfigurationFilePath())
                .CrossContext();

            var assemblyResolver = new DefaultAssemblyResolver();
            assemblyResolver.AddSearchDirectory(injectionBinder.GetInstance<IDirectory>().FullPath);
            injectionBinder.Bind<BaseAssemblyResolver>().To(assemblyResolver).CrossContext();

            injectionBinder.Bind<IGetCurrentStartupScene>().To<GetCurrentStartupScene>()
                .ToSingleton()
                .CrossContext();

            injectionBinder.Bind<IGetMonoBehavioursInScene>()
                .To<GetMonoBehavioursInScene>()
                .ToSingleton()
                .CrossContext();

            injectionBinder.Bind<IGetAddonTypesForScene>().To<GetAddonTypesForScene>()
                .ToSingleton()
                .CrossContext();

            injectionBinder.Bind<IMonoBehaviourFactory>().To<MonoBehaviourFactory>()
                .ToSingleton()
                .CrossContext();

            injectionBinder.Bind<IGetAttributesOfType<ReloadableAddonAttribute>>()
                .To<GetAttributesOfType<ReloadableAddonAttribute>>()
                .ToSingleton()
                .CrossContext();

            injectionBinder.Bind<IGetAttributesOfType<KSPScenario>>()
                .To<GetAttributesOfType<KSPScenario>>()
                .ToSingleton()
                .CrossContext();

            injectionBinder.Bind<IPartLoader>().Bind<IPartLoaderPrefabProvider>().To<KspPartLoader>().ToSingleton().CrossContext();
            injectionBinder.Bind<IGetPartModuleConfigsFromPartConfig>()
                .To<GetPartModuleConfigsFromPartConfig>()
                .ToSingleton()
                .CrossContext();

            // game events
            injectionBinder.Bind<SignalOnLevelWasLoaded>().ToSingleton().CrossContext();
        }


        public override void Launch()
        {
            base.Launch();
            injectionBinder.GetInstance<SignalStart>().Dispatch();
        } 
    }
}
