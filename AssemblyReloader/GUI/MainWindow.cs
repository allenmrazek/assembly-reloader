using System;
using System.Collections.Generic;
using AssemblyReloader.Annotations;
using AssemblyReloader.CompositeRoot;
using AssemblyReloader.Controllers;
using AssemblyReloader.Gui.Messages;
using ReeperCommon.Gui.Window;
using UnityEngine;

namespace AssemblyReloader.Gui
{
    public class MainWindow : BasicWindow
    {
        private readonly IController _controller;
        private readonly IEnumerable<IPluginInfo> _plugins;
        private readonly IMessageChannel _viewMessageChannel;
        [Persistent] private Vector2 _scroll = default(Vector2);


        public MainWindow(
            [NotNull] IController controller,
            [NotNull] IEnumerable<IPluginInfo> plugins,
            [NotNull] IMessageChannel viewMessageChannel,
            Rect rect, 
            int winid,
            GUISkin skin, 
            bool draggable = true) : base(rect, winid, skin, draggable)
        {
            if (controller == null) throw new ArgumentNullException("controller");
            if (plugins == null) throw new ArgumentNullException("plugins");
            if (viewMessageChannel == null) throw new ArgumentNullException("viewMessageChannel");

            _controller = controller;
            _plugins = plugins;
            _viewMessageChannel = viewMessageChannel;
        }


        public override void OnWindowDraw(int winid)
        {
            base.OnWindowDraw(winid);

            GUILayout.BeginVertical(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            {
                _scroll = GUILayout.BeginScrollView(_scroll, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
                {
                    DrawReloadableItems(_plugins);
                }
                GUILayout.EndScrollView();
            }
            GUILayout.EndVertical();
        }


        private void DrawReloadableItems(IEnumerable<IPluginInfo> items)
        {
            foreach (var item in items)
                DrawReloadableItem(item);
        }


        private void DrawReloadableItem(IPluginInfo plugin)
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label(plugin.Name);
                GUILayout.FlexibleSpace();

                if (GUILayout.Button("Options", GUILayout.ExpandWidth(false), GUILayout.ExpandHeight(false)))
                {
                    //_viewMessageChannel.Send(new ToggleOptionsWindow(plugin));
                    //_guiMediator.TogglePluginOptionsWindow(plugin);
                }

                if (GUILayout.Button("Reload", GUILayout.ExpandWidth(false), GUILayout.ExpandHeight(false)))
                    _controller.Reload(plugin);
                
                GUILayout.Space(3f); // just a bit of space, otherwise the button will overlap right side of scrollable area
            }
            GUILayout.EndHorizontal();
        }


        public void OnCloseButton()
        {
            Visible = false;
        }


        public void OnOptionsButton()
        {
            _viewMessageChannel.Send(new ShowOptionsWindow());
        }
    }
}
