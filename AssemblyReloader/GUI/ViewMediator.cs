using System;
using System.Collections.Generic;
using AssemblyReloader.StrangeIoC.extensions.injector;

namespace AssemblyReloader.Gui
{
// ReSharper disable once ClassNeverInstantiated.Global
    public class ViewMediator : IViewMediator
    {
        public IMainView MainView { private get; set; }
        public ISettingsView SettingsView { private get; set; }


        [Inject] public IEnumerable<IPluginInfo> Plugins { get; set; }

        public void Reload(IPluginInfo plugin)
        {
            if (plugin == null) throw new ArgumentNullException("plugin");

            throw new NotImplementedException();
        }

        public void TogglePluginOptions(IPluginInfo plugin)
        {
            if (plugin == null) throw new ArgumentNullException("plugin");

            throw new NotImplementedException();
        }


        public void ToggleOptions()
        {
            SettingsView.Visible = !SettingsView.Visible;
        }


        public void HideMainWindow()
        {
            MainView.Visible = SettingsView.Visible = false; // hide all windows
        }
    }
}
