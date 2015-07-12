using System;
using AssemblyReloader.Common;
using AssemblyReloader.Config;
using AssemblyReloader.Gui;
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

        public ReloadablePluginContext(MonoBehaviour view, IFile reloadableFile) : base(view)
        {
            if (reloadableFile == null) throw new ArgumentNullException("reloadableFile");

            _reloadableFile = reloadableFile;
        }


        protected override void mapBindings()
        {
            base.mapBindings();


            commandBinder.Bind<SignalStart>().To<CommandLoadPluginAssembly>().Once();

            //commandBinder.Bind<SignalLoadAssembly>().InSequence()
            //    .To<CommandLoadAddons>();

            // todo: map bindings for this file
        }


        public override void Launch()
        {
            injectionBinder.GetInstance<ILog>().Verbose("ReloadablePluginContext.Launch");
            base.Launch();
        }
    }
}
