using System;
using AssemblyReloader.Annotations;
using AssemblyReloader.Gui;
using ReeperCommon.Gui.Window;

namespace AssemblyReloader.Controllers
{

    public class ReloadablePluginController : IReloadablePluginController
    {
        private readonly IWindowComponent _configWindow;


        public ReloadablePluginController(
            IReloadablePlugin plugin, 
            [NotNull] IWindowComponent configWindow)
        {
            if (plugin == null) throw new ArgumentNullException("plugin");
            if (configWindow == null) throw new ArgumentNullException("configWindow");

            Plugin = plugin;
            _configWindow = configWindow;
        }


        public void Reload()
        {
            Plugin.Unload();
            LoadPlugin();
        }


        public void ToggleConfigurationView()
        {
            _configWindow.Visible = !_configWindow.Visible;
        }


        public IReloadablePlugin Plugin { get; private set; }



        private void LoadPlugin()
        {
            try
            {
                Plugin.Load();
            }
            catch (Exception)
            {
                throw;
            }
            
        }
    }
}