//using System;
//using System.Collections.Generic;
//using AssemblyReloader.Annotations;
//using AssemblyReloader.CompositeRoot;
//using AssemblyReloader.Controllers;
//using AssemblyReloader.DataObjects;
//using AssemblyReloader.Messages;
//using ReeperCommon.Gui.Logic;
//using UnityEngine;

//namespace AssemblyReloader.Gui
//{
//    public class PluginConfigurationViewLogic : IWindowLogic, IMessageConsumer<ToggleOptionsWindow>
//    {
//        private readonly IReloadablePlugin _plugin;
//        private readonly PluginConfiguration _pluginConfiguration;
//        private readonly List<IExpandablePanel> _panels = new List<IExpandablePanel>();
//        private Vector2 _scrollPos = default(Vector2);

//        public PluginConfigurationViewLogic(
//            [NotNull] IReloadablePlugin plugin,
//            [NotNull] PluginConfiguration pluginConfiguration)
//        {
//            if (plugin == null) throw new ArgumentNullException("plugin");
//            if (pluginConfiguration == null) throw new ArgumentNullException("pluginConfiguration");

//            _plugin = plugin;
//            _pluginConfiguration = pluginConfiguration;
//        }


//        public void Draw()
//        {
//            _scrollPos = GUILayout.BeginScrollView(_scrollPos, false, true);
//            foreach (var p in _panels) p.Draw(GUILayout.ExpandWidth(false), GUILayout.ExpandHeight(false));
//            GUILayout.EndScrollView();
//        }


//        public void AddPanel([NotNull] IExpandablePanel panel)
//        {
//            if (panel == null) throw new ArgumentNullException("panel");

//            _panels.Add(panel);
//        }


//        public void Update()
//        {

//        }

//        public void Consume([NotNull] ToggleOptionsWindow message)
//        {
//            if (message == null) throw new ArgumentNullException("message");
//            if (!ReferenceEquals(message.Plugin, _plugin)) return;

//            Visible = true;
//        }
//    }
//}
