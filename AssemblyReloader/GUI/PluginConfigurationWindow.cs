using System;
using AssemblyReloader.Annotations;
using AssemblyReloader.CompositeRoot;
using AssemblyReloader.Controllers;
using AssemblyReloader.Gui.Messages;
using ReeperCommon.Gui.Window;
using UnityEngine;

namespace AssemblyReloader.Gui
{
    public class PluginConfigurationWindow : BasicWindow, IMessageConsumer<ShowPluginConfigurationWindow>
    {
        private readonly IController _controller;
        private readonly IPluginInfo _plugin;

        public PluginConfigurationWindow(
            [NotNull] IController controller, 
            [NotNull] IPluginInfo plugin,
            Rect rect, 
            int winid, 
            GUISkin skin, 
            bool draggable = true) : base(rect, winid, skin, draggable)
        {
            if (controller == null) throw new ArgumentNullException("controller");
            if (plugin == null) throw new ArgumentNullException("plugin");
            _controller = controller;
            _plugin = plugin;
        }


        public override void OnWindowDraw(int winid)
        {
            base.OnWindowDraw(winid);


            _plugin.Configuration.StartAddonsForCurrentScene = GUILayout.Toggle(_plugin.Configuration.StartAddonsForCurrentScene, "Start addons for current scene");
            _plugin.Configuration.InstantlyAppliesToEveryScene = GUILayout.Toggle(_plugin.Configuration.InstantlyAppliesToEveryScene,
                "KSPAddon.Instantly applies to every scene");

            _plugin.Configuration.ReloadPartModulesImmediately = GUILayout.Toggle(_plugin.Configuration.ReloadPartModulesImmediately,
                "Reload PartModules immediately");

            _plugin.Configuration.ReusePartModuleConfigsFromPrevious =
                GUILayout.Toggle(_plugin.Configuration.ReusePartModuleConfigsFromPrevious, "Use snapshotted PartModule configs");


            _plugin.Configuration.ReloadScenarioModulesImmediately =
                GUILayout.Toggle(_plugin.Configuration.ReloadScenarioModulesImmediately, "Reload ScenarioModules immediately");

            _plugin.Configuration.SaveScenarioModuleConfigBeforeReloading =
                GUILayout.Toggle(_plugin.Configuration.SaveScenarioModuleConfigBeforeReloading,
                    "Serialize ScenarioModules before reloading");

            _plugin.Configuration.RewriteAssemblyLocationCalls = GUILayout.Toggle(_plugin.Configuration.RewriteAssemblyLocationCalls,
                "Intercept Assembly location method calls");

            _plugin.Configuration.WriteReweavedAssemblyToDisk = GUILayout.Toggle(_plugin.Configuration.WriteReweavedAssemblyToDisk,
                "Write reweaved assembly to disk");

        }


        public void OnCloseButton()
        {
            Visible = false;
            _controller.SavePluginConfiguration(_plugin);
        }


        public void Consume(ShowPluginConfigurationWindow message)
        {
            if (!ReferenceEquals(_plugin, message.Plugin))
                return;

            Visible = true;
        }
    }
}
