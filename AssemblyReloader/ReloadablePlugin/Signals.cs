using AssemblyReloader.Game;
using AssemblyReloader.StrangeIoC.extensions.signal.impl;

namespace AssemblyReloader.ReloadablePlugin
{
// ReSharper disable once ClassNeverInstantiated.Global
    public class SignalAssemblyLoaded: Signal<ILoadedAssemblyHandle>
    {
    }
}
