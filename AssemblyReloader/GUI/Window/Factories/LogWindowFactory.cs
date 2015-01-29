using System;
using AssemblyReloader.Logging;
using ReeperCommon.Gui.Window;
using ReeperCommon.Gui.Window.Providers;
using UnityEngine;

namespace AssemblyReloader.GUI.Window.Factories
{
    class LogWindowFactory
    {
        private readonly ICachedLog _cachedLog;

        public LogWindowFactory(ICachedLog cachedLog)
        {
            if (cachedLog == null) throw new ArgumentNullException("cachedLog");
            _cachedLog = cachedLog;
        }


        public IWindowComponent Create()
        {
            var logWindowLogic = new LogViewLogic(_cachedLog);
            var idProvider = new UniqueWindowIdProvider();

            var logWindow = new BasicWindow(logWindowLogic, new Rect(400f, 0f, 200f, 128f), idProvider.Get(),
                HighLogic.Skin, true) { Title = "ART Log", Visible = false };


            return logWindow;
        }
    }
}
