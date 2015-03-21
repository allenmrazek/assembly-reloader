using System.Collections.Generic;
using AssemblyReloader.PluginTracking;

namespace AssemblyReloader.Controllers
{
    public interface IReloadablePluginController
    {
        void Reload();
        void ToggleConfigurationView();

        IReloadablePlugin Plugin { get; }
    }
}
