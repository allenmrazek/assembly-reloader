extern alias KSP;
using System;
using ReeperCommon.Extensions;
using ReeperCommon.Gui;
using ReeperCommon.Gui.Window;
using ReeperCommon.Serialization;
using strange.extensions.mediation.impl;
using UnityEngine;
using ConfigNode = KSP::ConfigNode;

namespace AssemblyReloader.Gui
{
    public abstract class StrangeView : View, IWindowComponent
    {
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable RedundantDefaultFieldInitializer

        protected static readonly Vector2 TitleBarButtonOffset = new Vector2(2f, 2f);
        protected static readonly Vector2 ResizableHotzoneSize = new Vector2(10f, 10f);
        protected static readonly Vector2 MinWindowSize = new Vector2(200f, 100f);


        [ReeperPersistent] private Rect _windowRect = new Rect(0f, 0f, 200f, 300f);
        [ReeperPersistent] private WindowID _id = new WindowID();
        [ReeperPersistent] private string _title = string.Empty;
        [ReeperPersistent] private bool _draggable = false;
        [ReeperPersistent] private bool _visible = true;
        private IWindowComponent _logic;


        protected override void Start()
        {
            base.Start();

            _id = new WindowID(GetInstanceID());
            _logic = this; // just in case something goes wrong during init

            if (!registeredWithContext)
                throw new Exception("Failed to register with context");

            _logic = Initialize();
        }


        protected abstract IWindowComponent Initialize();
        protected abstract void DrawWindow();
        protected abstract void FinalizeWindow();


        #region window view


// ReSharper disable once UnusedMember.Local
// ReSharper disable once InconsistentNaming
        private void OnGUI()
        {
            
            if (_logic.IsNull() || !_logic.Visible) return;

            try
            {
                _logic.OnWindowPreDraw();

                if (!_logic.Skin.IsNull())
                    GUI.skin = _logic.Skin;


                _logic.Dimensions = GUILayout.Window(_logic.Id.Value, _logic.Dimensions, DrawWindow,
                    _logic.Title, Skin.window);
            }
            catch (Exception e)
            {
                CloseWindowDueToError("Exception in StrangeView.OnGUI", e);
                throw;
            }
        }


        private void DrawWindow(int winid)
        {
            try
            {
                _logic.OnWindowDraw(winid);
                _logic.OnWindowFinalize(winid);
            }
            catch (Exception e)
            {
                CloseWindowDueToError("Exception in StrangeView.DrawWindow", e);
                throw;
            }
        }


// ReSharper disable once UnusedMember.Local
        private void Update()
        {
            if (_logic.IsNull()) return;

            _logic.OnUpdate();
        }


        private void CloseWindowDueToError(string message, Exception e)
        {
            Visible = false;
            Debug.LogError(Title + ", id: " + Id +
               " has caused window to close: " + message + ", exception: " + e); 
        }

        #endregion


        #region IWindowComponent

        public virtual void OnWindowPreDraw()
        {
           
        }

        public virtual void OnWindowDraw(int winid)
        {
            if (!Skin.IsNull()) GUI.skin = Skin;

            DrawWindow();
        }


        public virtual void OnWindowFinalize(int winid)
        {
            if (Draggable) GUI.DragWindow();
            FinalizeWindow();
        }


        public virtual void OnUpdate()
        {

        }

        public virtual void DuringSerialize(IConfigNodeSerializer formatter, ConfigNode node)
        {
        }


        public virtual void DuringDeserialize(IConfigNodeSerializer formatter, ConfigNode node)
        {
        }


        public Rect Dimensions
        {
            get { return _windowRect; }
            set { _windowRect = value; }
        }


        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }


        public GUISkin Skin { get; set; }


        public bool Draggable
        {
            get { return _draggable; }
            set { _draggable = value; }
        }


        public bool Visible
        {
            get { return _visible; }
            set { _visible = value; }
        }


        public WindowID Id
        {
            get { return _id; }
// ReSharper disable once UnusedMember.Global
            set { _id = value; }
        }


        public float Width
        {
            get { return _windowRect.width; }
            set { _windowRect.width = value; }
        }

        public float Height
        {
            get { return _windowRect.height; }
            set { _windowRect.height = value; }
        }

        #endregion

    }
}
