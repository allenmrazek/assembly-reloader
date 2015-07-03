using AssemblyReloader.Gui;

namespace AssemblyReloader.Controllers
{
    // MainView interacts with this to make changes to Model
    public interface IController
    {
        void Reload(IPluginInfo plugin);
        void SaveConfiguration();
        void SavePluginConfiguration(IPluginInfo plugin);
    }
}
