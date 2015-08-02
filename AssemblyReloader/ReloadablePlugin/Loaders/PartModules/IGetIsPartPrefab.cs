using AssemblyReloader.Game;

namespace AssemblyReloader.ReloadablePlugin.Loaders.PartModules
{
    public interface IGetPartIsPrefab
    {
        bool Get(IPart part);
    }
}
