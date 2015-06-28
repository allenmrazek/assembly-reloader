using AssemblyReloader.DataObjects;
using AssemblyReloader.FileSystem;
using ReeperCommon.FileSystem;

namespace AssemblyReloader.ReloadablePlugin
{
    public interface IAssemblyProviderFactory
    {
        IAssemblyProvider Create(PluginConfiguration pluginConfiguration, IDirectory temporaryDirectory);
    }
}
