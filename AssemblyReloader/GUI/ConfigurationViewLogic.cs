using System;
using System.Collections.Generic;
using AssemblyReloader.Annotations;
using AssemblyReloader.DataObjects;
using ReeperCommon.Gui.Controls;
using ReeperCommon.Gui.Logic;
using UnityEngine;

namespace AssemblyReloader.Gui
{
    public class ConfigurationViewLogic : IWindowLogic
    {
        private readonly Configuration _configuration;
        private readonly List<ICustomControl> _controls = new List<ICustomControl>();
 

        public ConfigurationViewLogic(
            Configuration configuration)
        {
            if (configuration == null) throw new ArgumentNullException("configuration");

            _configuration = configuration;
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

            foreach (var p in _controls) p.Draw(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(false));
        }



        public void DrawAddonPanel()
        {
            GUILayout.Toggle(true, "Reload Addons instantly");
            GUILayout.Toggle(true, "KSPAddon.Instant applies to all scenes");
        }


        public void DrawPartModulePanel()
        {
            GUILayout.Toggle(false, "DrawPartModulePanel option 1");
        }


        public void DrawScenarioModulePanel()
        {
            GUILayout.Toggle(false, "DrawScenarioModulePanel option 1");
        }


        public void DrawIntermediateLanguagePanel()
        {
            GUILayout.Toggle(false, "DrawIntermediateLanguagePanel option 1");
        }


        public void DrawGeneralOptionsPanel()
        {
            GUILayout.Toggle(false, "General option 1");
        }


        public void AddControl([NotNull] ICustomControl control)
        {
            if (control == null) throw new ArgumentNullException("control");

            _controls.Add(control);
        }


        public void Update()
        {

        }
    }
}
