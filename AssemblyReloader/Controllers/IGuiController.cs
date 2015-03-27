using System.Collections.Generic;

namespace AssemblyReloader.Controllers
{
    public interface IGuiController
    {
        IEnumerable<IReloadablePlugin> Plugins { get; }

        void Reload(IReloadablePlugin plugin);
        void TogglePluginOptionsWindow(IReloadablePlugin plugin);
    }
}
