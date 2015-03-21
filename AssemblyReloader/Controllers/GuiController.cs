using System;
using System.Collections.Generic;
using AssemblyReloader.PluginTracking;

namespace AssemblyReloader.Controllers
{
    public class GuiController : IGuiController
    {
        private readonly Dictionary<IReloadablePlugin, IReloadablePluginController> _pluginControllers;

        public GuiController(Dictionary<IReloadablePlugin, IReloadablePluginController> pluginControllers)
        {
            if (pluginControllers == null) throw new ArgumentNullException("pluginControllers");
            _pluginControllers = pluginControllers;
        }


        public IEnumerable<IReloadablePlugin> Plugins
        {
            get { return _pluginControllers.Keys; }
        }


        public void Reload(IReloadablePlugin plugin)
        {
            _pluginControllers[plugin].Reload();
        }


        public void TogglePluginOptionsWindow(IReloadablePlugin plugin)
        {
            _pluginControllers[plugin].ToggleConfigurationView();
        }
    }
}
