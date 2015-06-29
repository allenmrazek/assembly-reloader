using System.Reflection;
using ReeperCommon.Containers;
using ReeperCommon.FileSystem;

namespace AssemblyReloader.Game
{
    public interface IGameAssemblyLoader
    {
        Maybe<ILoadedAssemblyHandle> AddToLoadedAssemblies(Assembly assembly, IFile location);
        void RemoveFromLoadedAssemblies(ILoadedAssemblyHandle handle);

        AssemblyLoader.LoadedAssembyList LoadedAssemblies { get; set; }
    }
}
