using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AssemblyReloader.Config;
using ReeperCommon.Gui.Logic;
using ReeperCommon.Gui.Window;
using UnityEngine;

namespace AssemblyReloader.GUI
{
    public class ConfigurationViewLogic : IWindowLogic
    {
        private readonly IConfiguration _configuration;

        public ConfigurationViewLogic(IConfiguration configuration)
        {
            if (configuration == null) throw new ArgumentNullException("configuration");

            _configuration = configuration;
        }


        public void Draw()
        {

            GUILayout.Toggle(true, "Reload PartModules instantly");
            GUILayout.Toggle(true, "Reload InternalModules instantly");
            GUILayout.Toggle(true, "Reload Addons instantly");
            GUILayout.Toggle(true, "Reload ScenarioModules instantly");

            GUILayout.Toggle(true, "Rewrite calls to GetExecutingAssembly Location & CodeBase");
            GUILayout.Toggle(true, "Rewrite GameEvent subscriptions using safe removal proxy");

            GUILayout.Toggle(true, "KSPAddon.Instant applies to all scenes");
        }


        public void Update()
        {

        }
    }
}
