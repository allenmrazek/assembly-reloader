using System.Reflection;
using ReeperCommon.Containers;
using ReeperCommon.FileSystem;

namespace AssemblyReloader.ReloadablePlugin.Definition
{
    public interface IAssemblyProvider
    {
        Maybe<Assembly> Get(IFile file);
    }
}
