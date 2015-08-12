extern alias KSP;
using GameScenes = KSP::GameScenes;

namespace AssemblyReloader.ReloadablePlugin.Loaders.Addons
{
    public interface IGetCurrentGameScene
    {
        GameScenes Get();
    }
}
