using System;
using AssemblyReloader.Game;
using AssemblyReloader.Game.Providers;
using AssemblyReloader.StrangeIoC.extensions.implicitBind;
using AssemblyReloader.StrangeIoC.extensions.injector.api;

namespace AssemblyReloader.ReloadablePlugin.Loaders
{
    [Implements(InjectionBindingScope.CROSS_CONTEXT, typeof(IGetCurrentStartupScene))]
// ReSharper disable once UnusedMember.Global
    public class GetCurrentStartupScene : IGetCurrentStartupScene
    {
        private readonly IGetStartupSceneFromGameScene _query;
        private readonly IGetCurrentGameScene _getCurrentGameScene;

        public GetCurrentStartupScene(IGetStartupSceneFromGameScene query, IGetCurrentGameScene getCurrentGameScene)
        {
            if (query == null) throw new ArgumentNullException("query");
            if (getCurrentGameScene == null) throw new ArgumentNullException("getCurrentGameScene");
            _query = query;
            _getCurrentGameScene = getCurrentGameScene;
        }


        public KSPAddon.Startup Get()
        {
            return _query.Get(_getCurrentGameScene.Get());
        }
    }
}
