using System;
using System.Collections.Generic;
using System.Linq;
using AssemblyReloader.Config.Keys;
using ReeperCommon.Gui.Window;
using ReeperCommon.Gui.Window.Buttons;
using ReeperCommon.Gui.Window.Decorators;
using strange.extensions.injector;
using strange.extensions.signal.impl;
using UnityEngine;

namespace AssemblyReloader.Gui
{

// ReSharper disable once ClassNeverInstantiated.Global
    public class MainView : StrangeView
    {

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable MemberCanBePrivate.Global

        [Inject(StyleKey.TitleBarButtonStyle)] public GUIStyle TitleBarButtonStyle { get; set; }
        [Inject(TextureNameKey.CloseButton)] public Texture2D CloseButtonTexture { get; set; }
        [Inject(TextureNameKey.SettingsButton)] public Texture2D SettingsButtonTexture { get; set; }
        [Inject(TextureNameKey.ResizeCursor)] public Texture2D ResizeCursorTexture { get; set; }

        [Inject] public GUISkin WindowSkin { get; set; }


        private IPluginInfo[] _plugins; // backing field to avoid creating garbage every OnGUI with foreach
        public IEnumerable<IPluginInfo> Plugins {
            get { return _plugins; }
            set { _plugins = (value ?? Enumerable.Empty<IPluginInfo>()).ToArray(); }
        }


        internal readonly Signal<IPluginInfo> ReloadRequested = new Signal<IPluginInfo>();
        internal readonly Signal<IPluginInfo> TogglePluginConfiguration = new Signal<IPluginInfo>();
        internal readonly Signal Close = new Signal();
        internal readonly Signal ToggleConfiguration = new Signal();

        private Vector2 _scroll = default(Vector2);
        private bool _firstLayout = true;

        protected override IWindowComponent Initialize()
        {
            //Skin = HighLogic.Skin;
            Skin = WindowSkin;
            Draggable = true;
            Height = 128f;

            if (Plugins == null)
                Plugins = Enumerable.Empty<IPluginInfo>();

            var scaling = new Scaling(this, Vector2.one);

            var clamp = new ClampToScreen(scaling);

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
// foreach would create garbage every frame due to iterator
// ReSharper disable once ForCanBeConvertedToForeach
                    for (var idx = 0; idx < _plugins.Length; ++idx)
                        DrawReloadableItem(_plugins[idx]);

                    if (Event.current.type == EventType.Repaint && _firstLayout)
                        CalculateIdealInitialWindowSize();
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
                    TogglePluginConfiguration.Dispatch(reloadable);
            }
            GUILayout.EndHorizontal();
        }


        protected override void FinalizeWindow()
        {
            // no op
        }


        private void CloseWindow()
        {
            Close.Dispatch();
        }


        private void CalculateIdealInitialWindowSize()
        {
            _firstLayout = false;

            if (!_plugins.Any())
                return; // if nothing was drawn, calculating min size gets a lot harder and it doesn't matter anyway

            var r = GUILayoutUtility.GetLastRect();

            var windowSize = Skin.window.CalcScreenSize(new Vector2(r.width + 
                Skin.scrollView.padding.left + 
                Skin.scrollView.padding.right +
                Skin.scrollView.margin.left +
                Skin.scrollView.margin.right, r.height)); // height not particularly important
            
            if (Width < windowSize.x)
                Width = windowSize.x;
        }
    }
}
