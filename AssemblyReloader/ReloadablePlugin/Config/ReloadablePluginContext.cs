using System;
using AssemblyReloader.Common;
using AssemblyReloader.Gui;
using AssemblyReloader.StrangeIoC.extensions.context.api;
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

        public ReloadablePluginContext(MonoBehaviour view, IFile reloadableFile) : base(view, ContextStartupFlags.MANUAL_MAPPING)
        {
            if (reloadableFile == null) throw new ArgumentNullException("reloadableFile");

            _reloadableFile = reloadableFile;
        }


        protected override void mapBindings()
        {
            base.mapBindings();

            injectionBinder.Bind<IFile>().To(_reloadableFile);

            injectionBinder.Bind<IReloadablePlugin>().Bind<IPluginInfo>().To<ReloadablePlugin>().ToSingleton();


            //commandBinder.Bind<SignalStart>().To<CommandLoadPluginAssembly>().Once();

            //commandBinder.Bind<SignalLoadAssembly>().InSequence()
            //    .To<CommandLoadAddons>();



            Plugin = injectionBinder.GetInstance<IReloadablePlugin>();
            Info = injectionBinder.GetInstance<IPluginInfo>();
        }


        public override void Launch()
        {
            injectionBinder.GetInstance<ILog>().Verbose("ReloadablePluginContext.Launch");
            base.Launch();
        }
    }
}
