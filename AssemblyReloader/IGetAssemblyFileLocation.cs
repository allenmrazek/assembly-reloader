using System.Reflection;
using ReeperCommon.Containers;
using ReeperKSP.FileSystem;

namespace AssemblyReloader
{
    public interface IGetAssemblyFileLocation
    {
        Maybe<IFile> Get(Assembly target);
    }
}
