using System.Reflection;
using ReeperCommon.Containers;
using ReeperCommon.FileSystem;

namespace AssemblyReloader.ReloadablePlugin.Weaving
{
    public interface IAssemblyProvider
    {
        Maybe<Assembly> Get();
    }
}
