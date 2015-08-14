extern alias KSP;
using strange.extensions.signal.impl;
using KSPAddon = KSP::KSPAddon;

namespace AssemblyReloader.Game
{
    public class SignalOnLevelWasLoaded : Signal<KSPAddon.Startup>
    {
    }
}
