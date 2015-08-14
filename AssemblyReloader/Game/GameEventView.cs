extern alias KSP;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;
using KSPAddon = KSP::KSPAddon;

namespace AssemblyReloader.Game
{
    public class GameEventView : View
    {
        internal readonly Signal<KSPAddon.Startup> LevelWasLoaded = new Signal<KSPAddon.Startup>();


// ReSharper disable once UnusedMember.Local
        private void OnLevelWasLoaded(int level)
        {
            print("GameEventView: OnLevelWasLoaded " + level);
            print("That's the same as " + ((KSPAddon.Startup) level));

            // note: apparently it's intended to be cast to KSPAddon.Startup; GameScenes doesn't quite match up
            // with what AddonLoader is doing
            LevelWasLoaded.Dispatch((KSPAddon.Startup)level);
        }
    }
}
