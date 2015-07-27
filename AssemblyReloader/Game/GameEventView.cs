using AssemblyReloader.StrangeIoC.extensions.mediation.impl;
using AssemblyReloader.StrangeIoC.extensions.signal.impl;

namespace AssemblyReloader.Game
{
    public class GameEventView : View
    {
        internal readonly Signal<GameScenes> LevelWasLoaded = new Signal<GameScenes>();


// ReSharper disable once UnusedMember.Local
        private void OnLevelWasLoaded(int level)
        {
            print("GameEventView: OnLevelWasLoaded " + level);
            LevelWasLoaded.Dispatch((GameScenes) level);
        }
    }
}
