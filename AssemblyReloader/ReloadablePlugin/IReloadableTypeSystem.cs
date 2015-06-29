using System.Reflection;
using AssemblyReloader.Game;
using ReeperCommon.FileSystem;

namespace AssemblyReloader.ReloadablePlugin
{
    public interface IReloadableTypeSystem
    {
        void CreateReloadableTypesFrom(ILoadedAssemblyHandle assembly);
        void DestroyReloadableTypesFrom(ILoadedAssemblyHandle assembly);
    }
}