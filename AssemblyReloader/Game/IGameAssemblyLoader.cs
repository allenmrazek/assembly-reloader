extern alias KSP;
using System.Reflection;
using ReeperCommon.FileSystem;

namespace AssemblyReloader.Game
{
    public interface IGameAssemblyLoader
    {
        ILoadedAssemblyHandle AddToLoadedAssemblies(Assembly assembly, IFile location);
        void RemoveFromLoadedAssemblies(ILoadedAssemblyHandle handle);

        KSP::AssemblyLoader.LoadedAssembyList LoadedAssemblies { get; set; }
    }
}
