using AssemblyReloader.Gui;

namespace AssemblyReloader.Controllers
{
    public interface IController
    {
        void Reload(IPluginInfo plugin);
        void SaveConfiguration();
    }
}
