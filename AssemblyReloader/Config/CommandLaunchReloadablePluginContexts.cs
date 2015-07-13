using System.Collections.Generic;
using System.Linq;
using AssemblyReloader.ReloadablePlugin.Config;
using AssemblyReloader.StrangeIoC.extensions.command.impl;
using AssemblyReloader.StrangeIoC.extensions.injector;
using ReeperCommon.Logging;

namespace AssemblyReloader.Config
{
// ReSharper disable once ClassNeverInstantiated.Global
    public class CommandLaunchReloadablePluginContexts : Command
    {
        [Inject] public ILog Log { set; get; }
        [Inject] public IEnumerable<ReloadablePluginContext> Contexts { get; set; }

        public override void Execute()
        {
            Contexts.ToList().ForEach(ctx => ctx.Launch());

            injectionBinder.Unbind<IEnumerable<ReloadablePluginContext>>();
        }
    }
}
