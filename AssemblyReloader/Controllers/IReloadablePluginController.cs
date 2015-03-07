using System.Collections.Generic;
using AssemblyReloader.PluginTracking;

namespace AssemblyReloader.Controllers
{
    public interface IReloadablePluginController
    {
        void Reload(IReloadablePlugin identity);
        void ReloadAll();

        IEnumerable<IReloadablePlugin> Plugins { get; }
    }
}
