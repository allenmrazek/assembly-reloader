using AssemblyReloader.Game;
using AssemblyReloader.StrangeIoC.extensions.signal.impl;
using ReeperCommon.FileSystem;

namespace AssemblyReloader.ReloadablePlugin.Config
{
    public class SignalLoadAssembly : Signal<IFile>
    {
    }

    public class SignalUnloadAssembly : Signal<ILoadedAssemblyHandle>
    {
    }

// ReSharper disable once ClassNeverInstantiated.Global
    public class SignalAssemblyWasLoaded: Signal<ILoadedAssemblyHandle>
    {
    }

    public class SignalAssemblyWasUnloaded : Signal
    {
    }
}
