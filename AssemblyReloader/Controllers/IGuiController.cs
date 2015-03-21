using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AssemblyReloader.PluginTracking;

namespace AssemblyReloader.Controllers
{
    public interface IGuiController
    {
        IEnumerable<IReloadablePlugin> Plugins { get; }

        void Reload(IReloadablePlugin plugin);
        void TogglePluginOptionsWindow(IReloadablePlugin plugin);
    }
}
