extern alias KSP;
using strange.extensions.signal.impl;
using KSPAddon = KSP::KSPAddon;

namespace AssemblyReloader.Game
{
// ReSharper disable once ClassNeverInstantiated.Global
    public class SignalOnLevelWasLoaded : Signal<KSPAddon.Startup>
    {
    }

// ReSharper disable once ClassNeverInstantiated.Global
    public class SignalApplicationQuitting : Signal
    {
        
    }
}
