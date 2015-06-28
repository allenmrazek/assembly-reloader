using AssemblyReloader.DataObjects;
using AssemblyReloader.FileSystem;

namespace AssemblyReloader.ReloadablePlugin
{
    public interface IAssemblyProviderFactory
    {
        IAssemblyProvider Create(PluginConfiguration pluginConfiguration);
    }
}
