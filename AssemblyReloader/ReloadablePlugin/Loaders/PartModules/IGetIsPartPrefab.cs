using AssemblyReloader.Game;

namespace AssemblyReloader.ReloadablePlugin.Loaders.PartModules
{
    public interface IGetIsPartPrefab
    {
        bool Get(IPart part);
    }
}
