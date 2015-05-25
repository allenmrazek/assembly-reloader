using System;
using System.Collections.Generic;
using AssemblyReloader.Annotations;
using AssemblyReloader.DataObjects;
using ReeperCommon.Gui.Logic;
using UnityEngine;

namespace AssemblyReloader.Gui
{
    public class PluginConfigurationViewLogic : IWindowLogic
    {
        private readonly PluginConfiguration _pluginConfiguration;
        private readonly List<IExpandablePanel> _panels = new List<IExpandablePanel>();
        private Vector2 _scrollPos = default(Vector2);

        public PluginConfigurationViewLogic(
            PluginConfiguration pluginConfiguration)
        {
            if (pluginConfiguration == null) throw new ArgumentNullException("pluginConfiguration");

            _pluginConfiguration = pluginConfiguration;
        }


        public void Draw()
        {
            _scrollPos = GUILayout.BeginScrollView(_scrollPos, false, true);
            foreach (var p in _panels) p.Draw(GUILayout.ExpandWidth(false), GUILayout.ExpandHeight(false));
            GUILayout.EndScrollView();
        }


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
