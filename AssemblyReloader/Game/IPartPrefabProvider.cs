using AssemblyReloader.ReloadablePlugin.Loaders;

namespace AssemblyReloader.Game
{
    public interface IPartPrefabProvider
    {
        IPart GetPrefab(IPart from);
    }
}
