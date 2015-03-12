using System.Reflection;
using ReeperCommon.Containers;
using ReeperCommon.FileSystem;

namespace AssemblyReloader.Game
{
    public interface IAssemblyLoader
    {
        Maybe<Assembly> Load();
        void Unload();
    }
}
