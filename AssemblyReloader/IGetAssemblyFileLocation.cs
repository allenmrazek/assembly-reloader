using System.Reflection;
using ReeperCommon.Containers;
using ReeperCommon.FileSystem;

namespace AssemblyReloader
{
    public interface IGetAssemblyFileLocation
    {
        Maybe<IFile> Get(Assembly target);
    }
}
