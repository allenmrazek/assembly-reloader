using AssemblyReloader.Game;
using AssemblyReloader.StrangeIoC.extensions.signal.impl;
using ReeperCommon.FileSystem;

namespace AssemblyReloader.ReloadablePlugin.Config
{
    public class SignalLoadReloadablePlugin : Signal
    {
    }

    public class SignalUnloadReloadablePlugin : Signal<ILoadedAssemblyHandle>
    {
    }
}
