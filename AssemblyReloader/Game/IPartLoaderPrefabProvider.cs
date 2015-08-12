using AssemblyReloader.ReloadablePlugin.Loaders;

namespace AssemblyReloader.Game
{
    public interface IPartLoaderPrefabProvider
    {
        IPart GetPrefab(IPart from);
    }
}
