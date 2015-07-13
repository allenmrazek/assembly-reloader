using System;
using System.Collections.Generic;
using AssemblyReloader.Config;
using AssemblyReloader.Config.Names;
using AssemblyReloader.StrangeIoC.extensions.injector;
using AssemblyReloader.StrangeIoC.extensions.mediation.impl;
using ReeperCommon.Extensions;
using ReeperCommon.Gui;
using ReeperCommon.Gui.Window;
using ReeperCommon.Gui.Window.Buttons;
using ReeperCommon.Gui.Window.Decorators;
using ReeperCommon.Serialization;
using UnityEngine;

namespace AssemblyReloader.Gui
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class MainView : View, IWindowComponent
    {
        [Inject(Styles.TitleBarButtonStyle)]
        public GUIStyle TitleBarButtonStyle { get; set; }

        [Inject(TextureNames.CloseButton)]
        public Texture2D CloseButtonTexture { get; set; }

        [Inject(TextureNames.SettingsButton)]
        public Texture2D SettingsButtonTexture { get; set; }

        [Inject(TextureNames.ResizeCursor)]
        public Texture2D ResizeCursorTexture { get; set; }


        public IEnumerable<IPluginInfo> Plugins { get; set; }
        private IWindowComponent _logic;


        [ReeperPersistent] private Rect _windowRect = new Rect(0f, 0f, 200f, 300f);
        [ReeperPersistent] private WindowID _id = new WindowID();
        [ReeperPersistent] private string _title = string.Empty;
        [ReeperPersistent] private bool _draggable = false;
        [ReeperPersistent] private bool _visible = true;

        private static readonly Vector2 TitleBarButtonOffset = new Vector2(2f, 2f);
        private static readonly Vector2 ResizableHotzoneSize = new Vector2(10f, 10f);
        private static readonly Vector2 MinWindowSize = new Vector2(200f, 100f);

        protected override void Start()
        {
            base.Start();



            _id = new WindowID(GetInstanceID());
            _logic = this; // just in case something goes wrong during init

            Skin = HighLogic.Skin;
            Draggable = true;
            
            if (!registeredWithContext)
                throw new Exception("Failed to register with context");

            var clamp = new ClampToScreen(this);

            var withButtons = new TitleBarButtons(clamp, TitleBarButtons.ButtonAlignment.Right, TitleBarButtonOffset);

            withButtons.AddButton(new BasicTitleBarButton(TitleBarButtonStyle, SettingsButtonTexture,
                ToggleSettings));

            withButtons.AddButton(new BasicTitleBarButton(TitleBarButtonStyle, CloseButtonTexture,
                Close));

            var resizable = new Resizable(withButtons, ResizableHotzoneSize, MinWindowSize, ResizeCursorTexture)
            {
                Title = "Assembly Reloader"
            };

            _logic = resizable;
        }


        #region window view


        private void OnGUI()
        {
            if (_logic.IsNull() || !_logic.Visible) return;

            if (!_logic.Skin.IsNull())
                GUI.skin = _logic.Skin;


            _logic.Dimensions = GUILayout.Window(_logic.Id.Value, _logic.Dimensions, DrawWindow,
                _logic.Title);

        }


        private void DrawWindow(int winid)
        {
            _logic.OnWindowDraw(winid);
            _logic.OnWindowFinalize(winid);
        }


        private void Update()
        {
            if (_logic.IsNull()) return;

            _logic.OnUpdate();
        }


        #endregion


        #region IWindowComponent


        public virtual void OnWindowDraw(int winid)
        {
            if (!Skin.IsNull()) GUI.skin = Skin;

            if (GUILayout.Button("Test button"))
                print("Test button clicked");
        }


        public virtual void OnWindowFinalize(int winid)
        {
            if (Draggable) GUI.DragWindow();
        }


        public virtual void OnUpdate()
        {

        }

        public virtual void Serialize(IConfigNodeSerializer formatter, ConfigNode node)
        {
            if (node == null) throw new ArgumentNullException("node");

            formatter.Serialize(this, node);
        }


        public virtual void Deserialize(IConfigNodeSerializer formatter, ConfigNode node)
        {
            if (node == null) throw new ArgumentNullException("node");

            formatter.Deserialize(this, node);
        }


        public Rect Dimensions
        {
            get { return _windowRect; }
            set { _windowRect = value; }
        }


        public string Title {
            get { return _title; } 
            set { _title = value; }
        }


        public GUISkin Skin { get; set; }

        
        public bool Draggable {
            get { return _draggable; }
            set { _draggable = value; }
        }

        
        public bool Visible {
            get { return _visible; }
            set { _visible = value; }
        }

        
        public WindowID Id {
            get { return _id; }
            set { _id = value; }
        }

        #endregion IWindowComponent








        //public override void OnWindowDraw(int winid)
        //{
        //    base.OnWindowDraw(winid);

        //    GUILayout.BeginVertical(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
        //    {
        //        _scroll = GUILayout.BeginScrollView(_scroll, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
        //        {
        //            foreach (var item in Plugins)
        //                DrawReloadableItem(item);
        //        }
        //        GUILayout.EndScrollView();
        //    }
        //    GUILayout.EndVertical();
        //}




        //private void DrawReloadableItem([NotNull] IPluginInfo reloadable)
        //{
        //    if (reloadable == null) throw new ArgumentNullException("reloadable");

        //    GUILayout.BeginHorizontal();
        //    {
        //        GUILayout.Label(reloadable.Name);
        //        GUILayout.FlexibleSpace();
        //        if (GUILayout.Button("Reload", GUILayout.ExpandWidth(false), GUILayout.ExpandHeight(false)))
        //        {
        //            //Mediator.Reload(reloadable);
        //        }
        //    }
        //    GUILayout.EndHorizontal();
        //}


        public void ToggleSettings()
        {
            //Mediator.ToggleOptions();
        }


        public void Close()
        {
            //Mediator.HideMainWindow();
        }
    }
}
