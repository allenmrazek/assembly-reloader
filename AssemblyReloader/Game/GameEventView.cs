using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;

namespace AssemblyReloader.Game
{
    public class GameEventView : View
    {
        internal readonly Signal<KSPAddon.Startup> LevelWasLoaded = new Signal<KSPAddon.Startup>();
        internal readonly Signal ApplicationQuit = new Signal();

// ReSharper disable once UnusedMember.Local
        private void OnLevelWasLoaded(int level)
        {
            // note: apparently it's intended to be cast to KSPAddon.Startup; GameScenes doesn't quite match up
            // with what AddonLoader is doing
            LevelWasLoaded.Dispatch((KSPAddon.Startup)level);
        }


// ReSharper disable once UnusedMember.Local
        private void OnApplicationQuit()
        {
            ApplicationQuit.Dispatch();
        }
    }
}
