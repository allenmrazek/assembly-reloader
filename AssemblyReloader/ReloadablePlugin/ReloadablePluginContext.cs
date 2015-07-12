using System;
using AssemblyReloader.Commands;
using AssemblyReloader.Config;
using AssemblyReloader.ReloadablePlugin.Commands;
using ReeperCommon.FileSystem;
using ReeperCommon.Logging;
using UnityEngine;

namespace AssemblyReloader.ReloadablePlugin
{
    public class ReloadablePluginContext : SignalContext
    {
        private readonly IFile _reloadableFile;

        public ReloadablePluginContext(MonoBehaviour view, IFile reloadableFile, bool autoStartup) : base(view, autoStartup)
        {
            if (reloadableFile == null) throw new ArgumentNullException("reloadableFile");

            _reloadableFile = reloadableFile;
        }


        protected override void mapBindings()
        {
            base.mapBindings();


            commandBinder.Bind<SignalStart>().To<CommandLoadPlugin>().Once();

            // todo: map bindings for this file
        }


        public override void Launch()
        {
            injectionBinder.GetInstance<ILog>().Verbose("ReloadablePluginContext.Launch");
            base.Launch();
        }
    }
}
