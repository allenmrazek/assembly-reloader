using ReeperAssemblyLibrary;
using ReeperCommon.Containers;

namespace AssemblyReloader.ReloadablePlugin.Loaders.Addons
{
    public interface IReloadableAddonLoader
    {
        void CreateAddons(KSPAddon.Startup scene);
        Maybe<ILoadedAssemblyHandle> Handle { get; set; }
    }
}
