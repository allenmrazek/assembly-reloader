using System;
using System.Collections.Generic;
using AssemblyReloader.Config;
using ReeperCommon.Gui.Controls;
using ReeperCommon.Gui.Logic;
using UnityEngine;

namespace AssemblyReloader.GUI
{
    public class ConfigurationViewLogic : IWindowLogic
    {
        private readonly IConfiguration _configuration;
        private readonly List<ICustomControl> _panels = new List<ICustomControl>();
 

        public ConfigurationViewLogic(
            IConfiguration configuration,
            IExpandablePanelFactory panelFactory)
        {
            if (configuration == null) throw new ArgumentNullException("configuration");
            if (panelFactory == null) throw new ArgumentNullException("panelFactory");

            _configuration = configuration;

            _panels.Add(panelFactory.Create("KSPAddon", DrawAddonConfigPanel));
            _panels.Add(panelFactory.Create("PartModule", () => { }));
            _panels.Add(panelFactory.Create("ScenarioModule", () => { }));
            _panels.Add(panelFactory.Create("Intermediate Language", () => { }));

        }


        public void Draw()
        {

            //GUILayout.Toggle(true, "Reload PartModules instantly");
            //GUILayout.Toggle(true, "Reload InternalModules instantly");
            //GUILayout.Toggle(true, "Reload ScenarioModules instantly");


            //GUILayout.Toggle(true, "PartModule configs are saved and then reloaded");

            //GUILayout.Toggle(true, "Reload all configs in directory or subdirectory");
            //GUILayout.Toggle(true, "Reload all textures in directory or subdirectory");
            //GUILayout.Toggle(true, "Reload all models in directory or subdirectory");


            //GUILayout.Toggle(true, "Rewrite calls to GetExecutingAssembly Location & CodeBase");
            //GUILayout.Toggle(true, "Rewrite GameEvent subscriptions using safe removal proxy");

            foreach (var p in _panels) p.Draw();
        }



        private void DrawAddonConfigPanel()
        {
            GUILayout.Toggle(true, "Reload Addons instantly");
            GUILayout.Toggle(true, "KSPAddon.Instant applies to all scenes");
        }


        public void Update()
        {

        }
    }
}
