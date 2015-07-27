using AssemblyReloader.Game.Providers;
using AssemblyReloader.StrangeIoC.extensions.implicitBind;

namespace AssemblyReloader.ReloadablePlugin.Loaders.Addons
{
    [Implements(typeof(IGetCurrentGameScene))]
// ReSharper disable once UnusedMember.Global
    public class GetCurrentGameScene : IGetCurrentGameScene
    {
        public GameScenes Get()
        {
            return HighLogic.LoadedScene;
        }
    }
}
