extern alias KSP;
extern alias Cecil96;
using System;
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
using AssemblyReloader.ReloadablePlugin.Weaving;
using AssemblyReloader.ReloadablePlugin.Weaving.Operations.GameEventInterception;
using ReeperAssemblyLibrary;
using ReeperCommon.FileSystem;
using ReeperCommon.Logging;
using strange.extensions.context.api;
using UnityEngine;
using Cecil96::Mono.Cecil;
using Object = UnityEngine.Object;

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



            SetupCommandBindings();
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

            injectionBinder.Bind<IGetAttributesOfType<KSP::KSPScenario>>()
                .To<GetAttributesOfType<KSP::KSPScenario>>()
                .ToSingleton()
                .CrossContext();

            injectionBinder.Bind<IPartLoader>().Bind<IPartLoaderPrefabProvider>().To<KspPartLoader>().ToSingleton().CrossContext();
            injectionBinder.Bind<IGetPartModuleConfigsFromPartConfig>()
                .To<GetPartModuleConfigsFromPartConfig>()
                .ToSingleton()
                .CrossContext();

            injectionBinder.Bind<IGetTypesDerivedFrom<KSP::PartModule>>()
                .To<GetTypesDerivedFrom<KSP::PartModule>>()
                .ToSingleton()
                .CrossContext();

            injectionBinder.Bind<IGetTypesDerivedFrom<KSP::ScenarioModule>>()
                .To<GetTypesDerivedFrom<KSP::ScenarioModule>>()
                .ToSingleton()
                .CrossContext();

            injectionBinder.Bind<IGetTypesDerivedFrom<KSP::VesselModule>>()
                .To<GetTypesDerivedFrom<KSP::VesselModule>>()
                .ToSingleton()
                .CrossContext();

            injectionBinder.Bind<IGameEventProxy>()
                .To(GameEventProxy.Create(injectionBinder.GetInstance<IGameEventReferenceFactory>(),
                    injectionBinder.GetInstance<ILog>().CreateTag("GameEventProxy")))
                .CrossContext();


            // game events
            injectionBinder.Bind<SignalOnLevelWasLoaded>().ToSingleton().CrossContext();
        }



        private void SetupCommandBindings()
        {
            commandBinder.Bind<SignalSaveWindow>()
                .To<CommandSaveWindowState>();

            commandBinder.Bind<SignalStart>()
                .InSequence()
                .To<CommandLoadConfiguration>()
                .To<CommandConfigureGameEvents>()
                .To<CommandConfigureGui>()
                .To<CommandLaunchReloadablePluginContexts>()
                .Once();
        }

        public override void Launch()
        {
            base.Launch();
            injectionBinder.GetInstance<SignalStart>().Dispatch();
        }



    }
}
