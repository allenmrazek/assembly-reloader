using System.Collections.Generic;
using System.Linq;
using AssemblyReloader.ReloadablePlugin.Config;
using ReeperCommon.Logging;
using strange.extensions.command.impl;
using strange.extensions.injector;

namespace AssemblyReloader.Config
{
// ReSharper disable once ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
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
