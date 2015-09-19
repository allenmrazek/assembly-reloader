extern alias KSP;

namespace AssemblyReloader.ReloadablePlugin.Loaders.PartModules
{
    public interface IPartModuleDestroyer
    {
        void Destroy(IPart owner, KSP::PartModule target);
    }
}
