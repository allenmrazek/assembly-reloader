using System.Reflection;

namespace AssemblyReloader.Loaders.AddonLoader
{
    public interface IAddonLoader
    {
        void CreateForScene(KSPAddon.Startup scene);
        void LoadAddonTypesFrom(Assembly assembly);
        void ClearAddonTypes(bool destroyLiveAddons = true);
        void DestroyLiveAddons();
      
    }
}
