using AssemblyReloader.StrangeIoC.extensions.implicitBind;

namespace AssemblyReloader.Game.Providers
{
    [Implements(typeof(ICurrentGameSceneProvider))]
// ReSharper disable once UnusedMember.Global
    public class CurrentGameSceneProvider : ICurrentGameSceneProvider
    {
        public GameScenes Get()
        {
            return HighLogic.LoadedScene;
        }
    }
}
