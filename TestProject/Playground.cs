using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ReeperCommon.Extensions;
using ReeperCommon.Logging.Implementations;
using UnityEngine;

namespace TestProject
{
    [KSPAddon(KSPAddon.Startup.SpaceCentre, true)]
    public class TestButton : MonoBehaviour
    {
        private ApplicationLauncherButton _button;

        private void Start()
        {
            var tex = new Texture2D(38, 38, TextureFormat.ARGB32, false);
            tex.GenerateRandom();

            Action empty = () => { };

            _button = ApplicationLauncher.Instance.AddModApplication(
                () => { },
                () => { },
                () => { },
                () => { },
                () => { },
                () => { },
                ApplicationLauncher.AppScenes.SPACECENTER, tex);

            ApplicationLauncher.Instance.EnableMutuallyExclusive(_button);
        }

        private void OnPluginReloadRequested()
        {
            print("Reload requested ...");
        }

        private void OnDestroy()
        {
            ApplicationLauncher.Instance.DisableMutuallyExclusive(_button);
            ApplicationLauncher.Instance.RemoveModApplication(_button);

        }
    }
}
