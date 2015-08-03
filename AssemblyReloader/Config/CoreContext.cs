using System.Collections.Generic;
using System.Linq;
using AssemblyReloader.Game;
using AssemblyReloader.Gui;
using AssemblyReloader.ReloadablePlugin;
using AssemblyReloader.ReloadablePlugin.Config;
using AssemblyReloader.ReloadablePlugin.Loaders;
using AssemblyReloader.ReloadablePlugin.Loaders.Addons;
using AssemblyReloader.ReloadablePlugin.Loaders.PartModules;
using AssemblyReloader.ReloadablePlugin.Weaving;
using AssemblyReloader.StrangeIoC.extensions.context.api;
using AssemblyReloader.Unsorted;
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

            MapCrossContextBindings(); // these bindings will be shared by the reloadable plugin contexts we're about to create

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
        }


        private void MapCrossContextBindings()
        {
            injectionBinder.Bind<GameObject>()
                .To(contextView as GameObject)
                .ToName(Keys.GameObjectKeys.CoreContext)
                .CrossContext();

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



            injectionBinder.Bind<IGetTypeIdentifier>().To<GetTypeIdentifier>().ToSingleton().CrossContext();
            //injectionBinder.Bind<IGameDatabase>().To<KspGameDatabase>().ToSingleton().CrossContext();
            injectionBinder.Bind<IGetUniqueFlightID>().To<GetUniqueFlightId>().ToSingleton().CrossContext();
            injectionBinder.Bind<IGetPartPrefabClones>().To<GetPartPrefabClones>().ToSingleton().CrossContext();
            injectionBinder.Bind<IGetPartIsPrefab>().To<GetPartIsPrefab>().ToSingleton().CrossContext();
            injectionBinder.Bind<IPartLoader>().To<KspPartLoader>().ToSingleton().CrossContext();
            injectionBinder.Bind<IGetConfigNodeForPart>().To<GetConfigNodeForPart>().ToSingleton().CrossContext();
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
