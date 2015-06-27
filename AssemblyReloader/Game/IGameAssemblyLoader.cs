using System.Reflection;
using ReeperCommon.FileSystem;

namespace AssemblyReloader.Game
{
    public interface IGameAssemblyLoader
    {
        ILoadedAssemblyHandle Load(Assembly assembly, IFile location);

        AssemblyLoader.LoadedAssembyList LoadedAssemblies { get; set; }
    }
}
