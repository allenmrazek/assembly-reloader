using System;
using UnityEngine;

namespace AssemblyReloader.CompositeRoot.MonoBehaviours
{
    [KSPAddon(KSPAddon.Startup.Instantly, true)]
// ReSharper disable once UnusedMember.Global
    class CoreView : MonoBehaviour
    {
        private Core _core;

// ReSharper disable once UnusedMember.Local
        private void Start()
        {
            try
            {
                _core = new Core();
                DontDestroyOnLoad(this);
            }
            catch
            {
                
            }
        }


        // Unity MonoBehaviour callback
// ReSharper disable once UnusedMember.Local
        // This is used to trigger OnLevelWasLoaded event instead of the GameEvent version
        // because the latter can trigger twice in a row, most likely due to a ksp bug
        private void OnLevelWasLoaded(int level)
        {
            _core.LevelWasLoaded(level);
        }
    }
}
