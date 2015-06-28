using System.Reflection;
using ReeperCommon.Containers;
using ReeperCommon.FileSystem;

namespace AssemblyReloader.FileSystem
{
    public interface IAssemblyProvider
    {
        Maybe<Assembly> Get(IFile file);
    }
}
