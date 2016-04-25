using ReeperAssemblyLibrary;
using ReeperCommon.Containers;
using ReeperCommon.Logging;
using ReeperKSP.FileSystem;
using strange.extensions.command.impl;

namespace AssemblyReloader.ReloadablePlugin.Config
{
// ReSharper disable once ClassNeverInstantiated.Global
    public class CommandStartReloadablePlugin : Command
    {
        [Inject]
        public SignalReloadPlugin ReloadPlugin { get; set; }

        [Inject]
        public IFile Plugin { get; set; }

        [Inject]
        public ILog Log { get; set; }

        // load plugin for the first time
        public override void Execute()
        {
            Log.Normal("First-load for plugin {0}", Plugin.Url);
            ReloadPlugin.Dispatch(Maybe<ILoadedAssemblyHandle>.None);
        }
    }
}
