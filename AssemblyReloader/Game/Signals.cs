extern alias KSP;
using AssemblyReloader.StrangeIoC.extensions.signal.impl;
using KSPAddon = KSP::KSPAddon;

namespace AssemblyReloader.Game
{
    public class SignalOnLevelWasLoaded : Signal<KSPAddon.Startup>
    {
    }
}
