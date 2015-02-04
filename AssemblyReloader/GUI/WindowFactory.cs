using System;
using System.Linq;
using AssemblyReloader.Logging;
using ReeperCommon.Gui.Window;
using ReeperCommon.Gui.Window.Buttons;
using ReeperCommon.Gui.Window.Decorators;
using ReeperCommon.Gui.Window.Providers;
using ReeperCommon.Repositories.Resources;
using UnityEngine;

namespace AssemblyReloader.GUI
{
    class WindowFactory
    {
        private readonly IResourceRepository _resourceProvider;
        private readonly ICachedLog _cachedLog;

        public WindowFactory(IResourceRepository resourceProvider, ICachedLog cachedLog)
        {
            if (resourceProvider == null) throw new ArgumentNullException("resourceProvider");
            if (cachedLog == null) throw new ArgumentNullException("cachedLog");

            _resourceProvider = resourceProvider;
            _cachedLog = cachedLog;
        }



        public IWindowComponent CreateMainWindow(MainViewWindowLogic logic)
        {
            if (logic == null) throw new ArgumentNullException("logic");

            var idProvider = new UniqueWindowIdProvider();


            var style = new GUIStyle(HighLogic.Skin.button) { border = new RectOffset(), padding = new RectOffset() };
            style.fixedHeight = style.fixedWidth = 16f;
            style.margin = new RectOffset();

            var btnClose = _resourceProvider.GetTexture("Resources/btnClose.png");



            var basicWindow = new BasicWindow(
                logic,
                new Rect(400f, 400f, 300f, 300f),
                idProvider.Get(), HighLogic.Skin /*AssetBase.GetGUISkin("KSP window 6")*/


                ) { Title = "ART: Assembly Reloading Tool" };


            var tbButtons = new TitleBarButtons(basicWindow, TitleBarButtons.ButtonAlignment.Right, new Vector2(3f, 3f));


            tbButtons.AddButton(new TitleBarButton(style, btnClose.First(), s => { }, "Test"));
            tbButtons.AddButton(new TitleBarButton(style, btnClose.First(), s => { }, "Test2"));
            tbButtons.AddButton(new TitleBarButton(style, btnClose.First(), s => { }, "Test3"));
            tbButtons.AddButton(new TitleBarButton(style, btnClose.First(), s => { }, "Tefsdst2"));
            tbButtons.AddButton(new TitleBarButton(style, btnClose.First(), s => { }, "Tefsdfdst2"));
            tbButtons.AddButton(new TitleBarButton(style, btnClose.First(), s => { }, "Tefddsffsdst2"));

            var hiding = new HideOnF2(tbButtons);

            var clamp = new ClampToScreen(hiding);

            return clamp;
        }



        public IWindowComponent CreateLogWindow()
        {
            var logWindowLogic = new LogViewLogic(_cachedLog);
            var idProvider = new UniqueWindowIdProvider();

            var logWindow = new BasicWindow(logWindowLogic, new Rect(400f, 0f, 200f, 128f), idProvider.Get(),
                HighLogic.Skin, true) { Title = "ART Log", Visible = false };


            return logWindow;
        }
    }
}
