using System;
using ReeperCommon.Extensions;
using UnityEngine;

namespace AssemblyReloader.CompositeRoot.MonoBehaviours
{
    [KSPAddon(KSPAddon.Startup.Instantly, true)]
// ReSharper disable once UnusedMember.Global
    class CoreView : LoadingSystem
    {
        private Core _core;

// ReSharper disable once UnusedMember.Local
        private void Start()
        {
            var ls = FindObjectOfType<LoadingScreen>();

            if (ls.IsNull())
            {
                Abort("AssemblyReloader failed to find LoadingScreen; aborting");
                return;
            }

            ls.loaders.Add(this);
            ls.loaders.ForEach(l => print("Loader: " + l.GetType().FullName));
        }


        private void Abort(string message)
        {
            Debug.LogError(message);
            Destroy(this);
        }


        public override bool IsReady()
        {
            return true;
        }


        // Our goal here is to load AFTER all part prefabs have been constructed
        public override void StartLoad()
        {
            try
            {
                _core = new Core();
                DontDestroyOnLoad(this);
            }
            catch (Exception e)
            {
                print("CoreView: Encountered an uncaught exception while creating Core: " + e);
                Destroy(this);
            }
        }
    }
}
