using System;
using System.Collections.Generic;
using AssemblyReloader.Controllers;
using AssemblyReloader.PluginTracking;
using ReeperCommon.Gui.Logic;
using UnityEngine;

namespace AssemblyReloader.GUI
{
    class View : IWindowLogic
    {
        private readonly IGuiController _controller;


        // gui
        private Vector2 _scroll = default(Vector2);



        public View(IGuiController controller)
        {
            if (controller == null) throw new ArgumentNullException("controller");

            _controller = controller;
        }


        public void Draw()
        {
            GUILayout.BeginVertical(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            {
                _scroll = GUILayout.BeginScrollView(_scroll, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
                {
                    DrawReloadableItems(_controller.Plugins);
                }
                GUILayout.EndScrollView();
            }
            GUILayout.EndVertical();
        }



        private void DrawReloadableItems(IEnumerable<IReloadablePlugin> items)
        {
            foreach (var item in items)
                DrawReloadableItem(item);
        }



        private void DrawReloadableItem(IReloadablePlugin plugin)
        {
            GUILayout.BeginHorizontal();
            {
                
                GUILayout.Label(plugin.Name);
                GUILayout.FlexibleSpace();

                if (GUILayout.Button("Options", GUILayout.ExpandWidth(false), GUILayout.ExpandHeight(false)))
                    _controller.TogglePluginOptionsWindow(plugin);

                if (GUILayout.Button("Reload", GUILayout.ExpandWidth(false), GUILayout.ExpandHeight(false)))
                    _controller.Reload(plugin);
                    

                GUILayout.Space(3f); // just a bit of space, otherwise the button will overlap right side of scrollable area
            }
            GUILayout.EndHorizontal();
        }




        public void Update()
        {
            //_log.Normal("ViewLogic.Update");
        }

    }
}
