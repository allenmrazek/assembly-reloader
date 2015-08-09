using System;
using System.Collections.Generic;
using System.Linq;
using AssemblyReloader.Game;
using AssemblyReloader.ReloadablePlugin.Loaders.Addons;
using AssemblyReloader.StrangeIoC.extensions.implicitBind;
using AssemblyReloader.StrangeIoC.extensions.injector.api;
using AssemblyReloader.Unsorted;

namespace AssemblyReloader.ReloadablePlugin.Loaders.ScenarioModules
{
    [Implements(typeof(IGetProtoScenarioModules), InjectionBindingScope.CROSS_CONTEXT)]
    public class GetProtoScenarioModules : IGetProtoScenarioModules
    {
        private readonly IGetTypeIdentifier _getTypeIdentifier;
        private readonly IGetCurrentGame _getCurrentGame;
        private readonly IGetCurrentGameScene _getCurrentGameScene;

        public GetProtoScenarioModules(
            IGetTypeIdentifier getTypeIdentifier,
            IGetCurrentGame getCurrentGame,
            IGetCurrentGameScene getCurrentGameScene)
        {
            if (getTypeIdentifier == null) throw new ArgumentNullException("getTypeIdentifier");
            if (getCurrentGame == null) throw new ArgumentNullException("getCurrentGame");
            if (getCurrentGameScene == null) throw new ArgumentNullException("getCurrentGameScene");

            _getTypeIdentifier = getTypeIdentifier;
            _getCurrentGame = getCurrentGame;
            _getCurrentGameScene = getCurrentGameScene;
        }


        public IEnumerable<IProtoScenarioModule> Get(Type type)
        {
            if (type == null) throw new ArgumentNullException("type");
            if (!_getCurrentGame.Get().Any()) return Enumerable.Empty<IProtoScenarioModule>(); //new IProtoScenarioModule[0];

            return _getCurrentGame.Get().Single().Scenarios
                .Where(psm => psm != null)
                .Where(psm => psm.moduleName == _getTypeIdentifier.Get(type).Identifier)
                .Where(psm => psm.TargetScenes.Contains(_getCurrentGameScene.Get()));

            //return HighLogic.CurrentGame.scenarios
            //    .Where(psm => psm != null)
            //    .Where(psm => psm.moduleName == _getTypeIdentifier.Get(type).Identifier)
            //    .Where(psm => psm.targetScenes.Contains(_getCurrentGameScene.Get()))
            //    .Select(psm => _kspFactory.Create(psm));
        }
    }
}
