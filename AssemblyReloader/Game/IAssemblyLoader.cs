using System.Reflection;
using ReeperCommon.Containers;

namespace AssemblyReloader.Game
{
    public interface IAssemblyLoader
    {
        Maybe<Assembly> Load();
        void Unload();
    }
}
