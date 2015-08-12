extern alias KSP;

namespace AssemblyReloader.ReloadablePlugin.Loaders
{
    public interface IGetStartupSceneFromGameScene
    {
        KSP::KSPAddon.Startup Get(KSP::GameScenes gameScene);
    }
}
