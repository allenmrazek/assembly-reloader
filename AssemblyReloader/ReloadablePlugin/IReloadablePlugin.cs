using AssemblyReloader.Gui;

namespace AssemblyReloader.ReloadablePlugin
{
    public interface IReloadablePlugin : IPluginInfo
    {
        void Reload();
    }
}
