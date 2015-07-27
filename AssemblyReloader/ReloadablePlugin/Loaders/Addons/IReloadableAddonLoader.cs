using AssemblyReloader.Game;
using ReeperCommon.Containers;

namespace AssemblyReloader.ReloadablePlugin.Loaders.Addons
{
    public interface IReloadableAddonLoader
    {
        void CreateAddons(GameScenes scene);
        Maybe<ILoadedAssemblyHandle> Handle { get; set; }
    }
}
