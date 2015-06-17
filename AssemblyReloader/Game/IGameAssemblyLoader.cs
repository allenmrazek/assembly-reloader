using ReeperCommon.Containers;
using ReeperCommon.FileSystem;

namespace AssemblyReloader.Game
{
    public interface IGameAssemblyLoader
    {
        //Maybe<Assembly> Deserialize();
        //void Unload();

        AssemblyLoader.LoadedAssembyList LoadedAssemblies { get; set; }
    }
}
