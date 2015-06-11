//using System;
//using System.Collections.Generic;
//using AssemblyReloader.Annotations;
//using AssemblyReloader.CompositeRoot;
//using AssemblyReloader.Controllers;
//using AssemblyReloader.Messages;
//using ReeperCommon.Gui.Logic;
//using UnityEngine;

//namespace AssemblyReloader.Gui
//{
//    class View : IWindowLogic
//    {
//        private readonly IGuiMediator _guiMediator;
//        private readonly IMessageChannel _viewChannel;
//        private readonly IEnumerable<IReloadablePlugin> _plugins;
//        private Vector2 _scroll = default(Vector2);



//        public View(
//            [NotNull] IGuiMediator guiMediator, 
//            [NotNull] IMessageChannel viewChannel, 
//            [NotNull] IEnumerable<IReloadablePlugin> plugins)
//        {
//            if (guiMediator == null) throw new ArgumentNullException("guiMediator");
//            if (viewChannel == null) throw new ArgumentNullException("viewChannel");
//            if (plugins == null) throw new ArgumentNullException("plugins");

//            _guiMediator = guiMediator;
//            _viewChannel = viewChannel;
//            _plugins = plugins;
//        }


//        public void Draw()
//        {
//            GUILayout.BeginVertical(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
//            {
//                _scroll = GUILayout.BeginScrollView(_scroll, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
//                {
//                    DrawReloadableItems(_plugins);
//                }
//                GUILayout.EndScrollView();
//            }
//            GUILayout.EndVertical();
//        }



//        private void DrawReloadableItems(IEnumerable<IReloadablePlugin> items)
//        {
//            foreach (var item in items)
//                DrawReloadableItem(item);
//        }



//        private void DrawReloadableItem(IReloadablePlugin plugin)
//        {
//            GUILayout.BeginHorizontal();
//            {
                
//                GUILayout.Label(plugin.Name);
//                GUILayout.FlexibleSpace();

//                if (GUILayout.Button("Options", GUILayout.ExpandWidth(false), GUILayout.ExpandHeight(false)))
//                    _viewChannel.Send(new ToggleOptionsWindow(plugin));
//                    //_guiMediator.TogglePluginOptionsWindow(plugin);

//                if (GUILayout.Button("Reload", GUILayout.ExpandWidth(false), GUILayout.ExpandHeight(false)))
//                    _guiMediator.Reload(plugin);
                    

//                GUILayout.Space(3f); // just a bit of space, otherwise the button will overlap right side of scrollable area
//            }
//            GUILayout.EndHorizontal();
//        }




//        public void Update()
//        {

//        }

//    }
//}
