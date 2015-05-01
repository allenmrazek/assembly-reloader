using ReeperCommon.Containers;
using ReeperCommon.FileSystem;

namespace AssemblyReloader.Game
{
    public interface IGameAssemblyLoader
    {
        //Maybe<Assembly> Load();
        //void Unload();

        AssemblyLoader.LoadedAssembyList LoadedAssemblies { get; set; }
    }
}
