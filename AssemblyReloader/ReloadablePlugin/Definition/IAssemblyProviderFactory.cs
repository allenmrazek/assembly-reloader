using AssemblyReloader.DataObjects;
using ReeperCommon.FileSystem;

namespace AssemblyReloader.ReloadablePlugin.Definition
{
    public interface IAssemblyProviderFactory
    {
        IAssemblyProvider Create(PluginConfiguration pluginConfiguration, IDirectory temporaryDirectory);
    }
}
