extern alias KSP;
using AssemblyReloader.Game;
using ReeperCommon.Containers;

namespace AssemblyReloader.ReloadablePlugin.Loaders.Addons
{
    public interface IReloadableAddonLoader
    {
        void CreateAddons(KSP::KSPAddon.Startup scene);
        Maybe<ILoadedAssemblyHandle> Handle { get; set; }
    }
}
