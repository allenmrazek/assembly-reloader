using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;
using UnityEngine.SceneManagement;

namespace AssemblyReloader.Game
{
    public class GameEventView : View
    {
        internal readonly Signal<KSPAddon.Startup> LevelWasLoaded = new Signal<KSPAddon.Startup>();
        internal readonly Signal ApplicationQuit = new Signal();

        // ReSharper disable once UnusedMember.Local
        private new void Start()
        {
            base.Start();
            SceneManager.activeSceneChanged += OnSceneChanged;
        }

        // ReSharper disable once UnusedMember.Local
        private new void OnDestroy()
        {
            SceneManager.activeSceneChanged -= OnSceneChanged;
            base.OnDestroy();
        }

        private void OnSceneChanged(Scene from, Scene to)
        {
            // note: apparently it's intended to be cast to KSPAddon.Startup; GameScenes doesn't quite match up
            // with what AddonLoader is doing
            LevelWasLoaded.Dispatch((KSPAddon.Startup)to.buildIndex);
        }


// ReSharper disable once UnusedMember.Local
        private void OnApplicationQuit()
        {
            ApplicationQuit.Dispatch();
        }
    }
}
