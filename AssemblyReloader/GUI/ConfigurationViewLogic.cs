using System;
using System.Collections.Generic;
using AssemblyReloader.Annotations;
using AssemblyReloader.DataObjects;
using ReeperCommon.Gui.Logic;
using UnityEngine;

namespace AssemblyReloader.Gui
{
    public class ConfigurationViewLogic : IWindowLogic
    {
        private readonly Configuration _configuration;
        private readonly List<IExpandablePanel> _panels = new List<IExpandablePanel>();
        private Vector2 _scrollPos = default(Vector2);

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

            _scrollPos = GUILayout.BeginScrollView(_scrollPos, false, true);
            foreach (var p in _panels) p.Draw(GUILayout.ExpandWidth(false), GUILayout.ExpandHeight(false));
            GUILayout.EndScrollView();
        }



        //public void DrawAddonPanel(IEnumerable<GUILayoutOption> options)
        //{
        //    _configuration.StartAddonsForCurrentScene = GUILayout.Toggle(_configuration.StartAddonsForCurrentScene, "Reload KSPAddons on the fly");
        //    _configuration.InstantlyAppliesToEveryScene = GUILayout.Toggle(_configuration.InstantlyAppliesToEveryScene, "KSPAddon.Instant applies to all scenes");
        //}


        //public void DrawPartModulePanel(IEnumerable<GUILayoutOption> options)
        //{
        //    _configuration.ReloadPartModulesImmediately = GUILayout.Toggle(_configuration.ReloadPartModulesImmediately, "Swap PartModules on the fly");
        //    _configuration.ReusePartModuleConfigsFromPrevious = GUILayout.Toggle(_configuration.ReusePartModuleConfigsFromPrevious, "Attempt to reuse PartModule ConfigNodes when swapping");
        //}


        //public void DrawScenarioModulePanel(IEnumerable<GUILayoutOption> options)
        //{
        //    _configuration.ReloadScenarioModulesImmediately =
        //        GUILayout.Toggle(_configuration.ReloadScenarioModulesImmediately, "Reload ScenarioModules on the fly");
        //    _configuration.SaveScenarioModuleConfigBeforeReloading = GUILayout.Toggle(false, "Attempt to reuse ScenarioModule ConfigNodes when swapping");
        //}


        //public void DrawIntermediateLanguagePanel(IEnumerable<GUILayoutOption> options)
        //{
        //    GUILayout.Toggle(false, "Intercept Assembly.CodeBase calls");
        //    GUILayout.Toggle(false, "Intercept Assembly.Location calls");
        //    GUILayout.Toggle(false, "Intercept GameEvent registrations");
        //}


        //public void DrawGeneralOptionsPanel(IEnumerable<GUILayoutOption> options)
        //{
        //    GUILayout.Toggle(true, "Reload KSPAddons");
        //    GUILayout.Toggle(true, "Reload PartModules");
        //    GUILayout.Toggle(true, "Reload ScenarioModules");
        //    GUILayout.Toggle(false, "Reload Contracts");
        //    GUILayout.Toggle(false, "Reload ExperienceTraits");
        //    GUILayout.Toggle(false, "Reload ExperienceEffects");
        //}


        public void AddPanel([NotNull] IExpandablePanel panel)
        {
            if (panel == null) throw new ArgumentNullException("panel");

            _panels.Add(panel);
        }


        public void Update()
        {

        }
    }
}
