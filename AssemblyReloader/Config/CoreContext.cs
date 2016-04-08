extern alias Cecil96;
using System;
using AssemblyReloader.Game;
using AssemblyReloader.Gui;
using AssemblyReloader.ReloadablePlugin;
using AssemblyReloader.ReloadablePlugin.Loaders;
using AssemblyReloader.ReloadablePlugin.Loaders.Addons;
using AssemblyReloader.ReloadablePlugin.Loaders.PartModules;
using AssemblyReloader.ReloadablePlugin.Weaving.Operations.GameEventInterception;
using ReeperAssemblyLibrary;
using ReeperCommon.Logging;
using strange.extensions.context.api;
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

            injectionBinder.Bind<SignalOnLoadConfiguration>().ToSingleton();
            injectionBinder.Bind<SignalOnSaveConfiguration>().ToSingleton();

            mediationBinder.BindView<MainView>().ToMediator<MainViewMediator>();
            mediationBinder.BindView<ConfigurationView>().ToMediator<ConfigurationViewMediator>();
            mediationBinder.BindView<ApplicationLauncherView>().ToMediator<ApplicationLauncherMediator>();
            mediationBinder.BindView<GameEventView>().ToMediator<GameEventMediator>();
            

            SetupCommandBindings();
        }


        private void MapCrossContextBindings()
        {
            var reeperResolver = new ReeperAssemblyResolver();
            AppDomain.CurrentDomain.AssemblyResolve += reeperResolver.Resolve;
            injectionBinder.Bind<IReeperAssemblyResolver>().ToValue(reeperResolver).CrossContext();

            injectionBinder.Bind<IGetConfigurationFilePath>()
                .To(new GetConfigurationFilePath())
                .CrossContext();


            injectionBinder.Bind<IRoutineRunner>().To<RoutineRunner>().ToSingleton().CrossContext();

            injectionBinder.Bind<IGetCurrentStartupScene>().To<GetCurrentStartupScene>()
                .ToSingleton()
                .CrossContext();

            injectionBinder.Bind<IGetMonoBehavioursInScene>()
                .To<GetMonoBehavioursInScene>()
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

            injectionBinder.Bind<IPartLoader>().Bind<IPartPrefabProvider>().To<KspPartLoader>().ToSingleton().CrossContext();
            injectionBinder.Bind<IGetPartModuleConfigsFromPartConfig>()
                .To<GetPartModuleConfigsFromPartConfig>()
                .ToSingleton()
                .CrossContext();

            injectionBinder.Bind<IGetTypesDerivedFrom<PartModule>>()
                .To<GetTypesDerivedFrom<PartModule>>()
                .ToSingleton()
                .CrossContext();

            injectionBinder.Bind<IGetTypesDerivedFrom<ScenarioModule>>()
                .To<GetTypesDerivedFrom<ScenarioModule>>()
                .ToSingleton()
                .CrossContext();

            injectionBinder.Bind<IGetTypesDerivedFrom<VesselModule>>()
                .To<GetTypesDerivedFrom<VesselModule>>()
                .ToSingleton()
                .CrossContext();

            injectionBinder.Bind<IGameEventProxy>()
                .To(GameEventProxy.Create(injectionBinder.GetInstance<IGameEventReferenceFactory>(),
                    injectionBinder.GetInstance<ILog>().CreateTag("GameEventProxy")))
                .CrossContext();

            injectionBinder.Bind<IGetConfigNodeForPart>().To<GetConfigNodeForPart>().ToSingleton().CrossContext();

            // game events
            injectionBinder.Bind<SignalOnLevelWasLoaded>().ToSingleton().CrossContext();
            injectionBinder.Bind<SignalApplicationQuitting>().ToSingleton().CrossContext();
            injectionBinder.Bind<SignalGameDatabaseReloadTriggered>().ToSingleton().CrossContext();
        }


        private void SetupCommandBindings()
        {
            commandBinder.Bind<SignalApplicationQuitting>()
                .To<CommandSaveCoreConfiguration>();

            commandBinder.Bind<SignalStart>()
                .InSequence() 
                .To<CommandConfigureCecilAssemblyResolver>()
                .To<CommandBootstrapReloadablePluginContexts>()
                .To<CommandConfigureGameEvents>()
                .To<CommandCreateGui>()
                .To<CommandLoadCoreConfiguration>()
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
