using AssemblyReloader.Config;
using AssemblyReloader.DataObjects;
using AssemblyReloader.ReloadablePlugin.Weaving;
using ReeperCommon.FileSystem;

namespace AssemblyReloader.ReloadablePlugin.Definition
{
    public interface IAssemblyProviderFactory
    {
        IAssemblyProvider Create(PluginConfiguration pluginConfiguration, IDirectory temporaryDirectory);
    }
}
