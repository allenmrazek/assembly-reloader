extern alias KSP;
using AssemblyReloader.Game;

namespace AssemblyReloader.ReloadablePlugin.Loaders.PartModules
{
    public interface IPartModuleDestroyer
    {
        void Destroy(IPart owner, KSP::PartModule target);
    }
}
