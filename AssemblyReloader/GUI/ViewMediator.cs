using System;
using System.Collections.Generic;
using AssemblyReloader.Config;
using AssemblyReloader.StrangeIoC.extensions.injector;

namespace AssemblyReloader.Gui
{
// ReSharper disable once ClassNeverInstantiated.Global
    public class ViewMediator : IViewMediator
    {
        private readonly IEnumerable<IPluginInfo> _plugins;
        private readonly IConfigurationSaver _mainConfigurationSaver;


        public IMainView MainView { private get; set; }
        public ISettingsView SettingsView { private get; set; }


        //[Inject] public IEnumerable<IPluginInfo> Plugins { get; set; }
        //[Inject] public IConfigurationSaver ConfigurationSaver { get; set; }


        public ViewMediator(IEnumerable<IPluginInfo> plugins, IConfigurationSaver mainConfigurationSaver)
        {
            if (plugins == null) throw new ArgumentNullException("plugins");
            if (mainConfigurationSaver == null) throw new ArgumentNullException("mainConfigurationSaver");
            _plugins = plugins;
            _mainConfigurationSaver = mainConfigurationSaver;
        }


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


        public void SaveConfiguration()
        {
            _mainConfigurationSaver.Save();
        }
    }
}
