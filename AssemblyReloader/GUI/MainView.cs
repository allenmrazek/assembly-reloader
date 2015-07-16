﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AssemblyReloader.Config;
using AssemblyReloader.StrangeIoC.extensions.signal.impl;
using ReeperCommon.Gui.Window;
using ReeperCommon.Gui.Window.Buttons;
using ReeperCommon.Gui.Window.Decorators;
using UnityEngine;

namespace AssemblyReloader.Gui
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class MainView : StrangeView
    {
        public IEnumerable<IPluginInfo> Plugins { get; set; }


        internal readonly Signal<IPluginInfo> ReloadRequested = new Signal<IPluginInfo>();
        internal readonly Signal<IPluginInfo> TogglePluginOptions = new Signal<IPluginInfo>();
        internal readonly Signal Close = new Signal();
        internal readonly Signal ToggleConfiguration = new Signal();

        private Vector2 _scroll = default(Vector2);


        protected override IWindowComponent Initialize()
        {
            Skin = HighLogic.Skin;
            Draggable = true;

            if (Plugins == null)
                Plugins = Enumerable.Empty<IPluginInfo>();
       
            var clamp = new ClampToScreen(this);

            var withButtons = new TitleBarButtons(clamp, TitleBarButtons.ButtonAlignment.Right, TitleBarButtonOffset);

            withButtons.AddButton(new BasicTitleBarButton(TitleBarButtonStyle, SettingsButtonTexture,
                () => ToggleConfiguration.Dispatch()));

            withButtons.AddButton(new BasicTitleBarButton(TitleBarButtonStyle, CloseButtonTexture, CloseWindow));

            var resizable = new Resizable(withButtons, ResizableHotzoneSize, MinWindowSize, ResizeCursorTexture)
            {
                Title = "Assembly Reloader"
            };

            return resizable;
        }


        protected override void DrawWindow()
        {
            GUILayout.BeginVertical(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            {
                _scroll = GUILayout.BeginScrollView(_scroll, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
                {
                    foreach (var item in Plugins)
                        DrawReloadableItem(item);
                }
                GUILayout.EndScrollView();
            }
            GUILayout.EndVertical();
        }


        private void DrawReloadableItem(IPluginInfo reloadable)
        {
            if (reloadable == null) throw new ArgumentNullException("reloadable");

            GUILayout.BeginHorizontal();
            {
                GUILayout.Label(reloadable.Name);
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Reload", GUILayout.ExpandWidth(false), GUILayout.ExpandHeight(false)))
                    ReloadRequested.Dispatch(reloadable);

                if (GUILayout.Button("Options", GUILayout.ExpandWidth(false), GUILayout.ExpandHeight(false)))
                    TogglePluginOptions.Dispatch(reloadable);
            }
            GUILayout.EndHorizontal();
        }


        protected override void FinalizeWindow()
        {
            // no op
        }


        private void CloseWindow()
        {
            // main view closes all windows when closed
            Close.Dispatch();
        }
    }
}