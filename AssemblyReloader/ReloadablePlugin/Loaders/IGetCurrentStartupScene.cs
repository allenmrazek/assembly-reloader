extern alias KSP;

namespace AssemblyReloader.ReloadablePlugin.Loaders
{
// ReSharper disable once InconsistentNaming
    public interface IGetCurrentStartupScene
    {
        KSP::KSPAddon.Startup Get();
    }
}
