extern alias KSP;
using System;
using AssemblyReloader.ReloadablePlugin.Loaders.Addons;
using strange.extensions.implicitBind;
using strange.extensions.injector.api;
using GameScenes = KSP::GameScenes;

namespace AssemblyReloader.ReloadablePlugin.Loaders.VesselModules
{
    [Implements(typeof(IGetCurrentSceneIsFlight), InjectionBindingScope.CROSS_CONTEXT)]
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
