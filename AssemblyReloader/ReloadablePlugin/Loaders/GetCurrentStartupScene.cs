extern alias KSP;
using System;
using AssemblyReloader.ReloadablePlugin.Loaders.Addons;
using strange.extensions.implicitBind;
using strange.extensions.injector.api;

namespace AssemblyReloader.ReloadablePlugin.Loaders
{
    [Implements(InjectionBindingScope.CROSS_CONTEXT, typeof(IGetCurrentStartupScene))]
// ReSharper disable once UnusedMember.Global
// ReSharper disable once ClassNeverInstantiated.Global
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


        public KSP::KSPAddon.Startup Get()
        {
            return _query.Get(_getCurrentGameScene.Get());
        }
    }
}
