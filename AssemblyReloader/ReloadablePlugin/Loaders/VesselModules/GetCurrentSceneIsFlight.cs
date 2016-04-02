using System;
using AssemblyReloader.ReloadablePlugin.Loaders.Addons;
using strange.extensions.injector.api;

namespace AssemblyReloader.ReloadablePlugin.Loaders.VesselModules
{
    [Implements(typeof(IGetCurrentSceneIsFlight), InjectionBindingScope.CROSS_CONTEXT)]
// ReSharper disable once UnusedMember.Global
    public class GetCurrentSceneIsFlight : IGetCurrentSceneIsFlight
    {
        private readonly IGetCurrentGameScene _gameScene;

        public GetCurrentSceneIsFlight(IGetCurrentGameScene gameScene)
        {
            if (gameScene == null) throw new ArgumentNullException("gameScene");

            _gameScene = gameScene;
        }


        public bool Get()
        {
            return _gameScene.Get() == GameScenes.FLIGHT;
        }
    }
}
