using AssemblyReloader.StrangeIoC.extensions.implicitBind;

namespace AssemblyReloader.Game.Providers
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
