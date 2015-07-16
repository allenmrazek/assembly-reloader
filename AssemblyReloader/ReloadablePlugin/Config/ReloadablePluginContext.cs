using System;
using AssemblyReloader.Common;
using AssemblyReloader.Config.Keys;
using AssemblyReloader.Gui;
using AssemblyReloader.ReloadablePlugin.Weaving;
using AssemblyReloader.StrangeIoC.extensions.context.api;
using Mono.Cecil;
using ReeperCommon.FileSystem;
using ReeperCommon.Logging;
using UnityEngine;

namespace AssemblyReloader.ReloadablePlugin.Config
{
    public class ReloadablePluginContext : SignalContext
    {
        private readonly IFile _reloadableFile;

        public IReloadablePlugin Plugin { get; private set; }
        public IPluginInfo Info { get; private set; }

        public ReloadablePluginContext(MonoBehaviour view, IFile reloadableFile) : base(view, ContextStartupFlags.MANUAL_MAPPING | ContextStartupFlags.MANUAL_LAUNCH)
        {
            if (reloadableFile == null) throw new ArgumentNullException("reloadableFile");

            _reloadableFile = reloadableFile;
        }


        protected override void mapBindings()
        {
            base.mapBindings();


            injectionBinder.Bind<IFile>().To(_reloadableFile);
            injectionBinder.Bind<ILog>()
                .To(injectionBinder.GetInstance<ILog>().CreateTag(_reloadableFile.Name))
                .ToName(LogKeys.PluginContext);

            injectionBinder.Bind<IReloadablePlugin>().Bind<IPluginInfo>().To<ReloadablePlugin>().ToSingleton();

            // only happens once: initial load of reloadable plugin
            commandBinder.Bind<SignalStart>()
                .To<CommandStartReloadablePlugin>().Once();


            commandBinder.Bind<SignalLoadReloadablePlugin>()
                .To<CommandReadDefinition>();

            commandBinder.Bind<SignalDefinitionWasRead>()
                .To<CommandTest>(); // todo: alterations to assembly definition here


            commandBinder.Bind<SignalUnloadReloadablePlugin>()
                .To<CommandNull>();




            Plugin = injectionBinder.GetInstance<IReloadablePlugin>();
            Info = injectionBinder.GetInstance<IPluginInfo>();
        }


        public override void Launch()
        {
            injectionBinder.GetInstance<ILog>().Normal(Plugin.Name + " context launching");
            base.Launch();
            injectionBinder.GetInstance<SignalStart>().Dispatch();
        }
    }
}
