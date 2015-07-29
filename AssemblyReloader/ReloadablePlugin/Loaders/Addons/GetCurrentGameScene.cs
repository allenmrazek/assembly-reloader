using AssemblyReloader.StrangeIoC.extensions.implicitBind;
using UnityEngine;

namespace AssemblyReloader.ReloadablePlugin.Loaders.Addons
{
    [Implements(typeof(IGetCurrentGameScene))]
// ReSharper disable once UnusedMember.Global
    public class GetCurrentGameScene : IGetCurrentGameScene
    {
        public GameScenes Get()
        {
            // ER: HighLogic.LoadedLevel might look like the right thing but it'll return incorrect
            // results, especially noticable when first loading the game
            return (GameScenes)Application.loadedLevel;
        }
    }
}
