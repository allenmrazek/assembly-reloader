using System.Reflection;
using ReeperCommon.Containers;
using ReeperCommon.FileSystem;

namespace AssemblyReloader.Providers
{
    public interface IAssemblyProvider
    {
        Maybe<Assembly> Get(IFile file);
    }
}
