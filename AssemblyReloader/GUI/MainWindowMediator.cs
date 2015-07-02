using System;
using System.Collections.Generic;
using AssemblyReloader.StrangeIoC.extensions.injector;

namespace AssemblyReloader.Gui
{
// ReSharper disable once ClassNeverInstantiated.Global
    public class MainWindowMediator : IMainViewMediator
    {
        // View injects itself here
        public IMainView View { get; set; }

        [Inject] public IEnumerable<IPluginInfo> Plugins { get; set; }


        public void Reload(IPluginInfo plugin)
        {
            if (plugin == null) throw new ArgumentNullException("plugin");

            throw new System.NotImplementedException();
        }

        public void ToggleOptions(IPluginInfo plugin)
        {
            if (plugin == null) throw new ArgumentNullException("plugin");

            throw new System.NotImplementedException();
        }
    }
}
