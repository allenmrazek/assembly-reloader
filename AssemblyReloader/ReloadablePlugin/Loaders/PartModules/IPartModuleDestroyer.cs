using AssemblyReloader.Game;

namespace AssemblyReloader.ReloadablePlugin.Loaders.PartModules
{
    public interface IPartModuleDestroyer
    {
        void Destroy(IPart owner, PartModule target);
    }
}
