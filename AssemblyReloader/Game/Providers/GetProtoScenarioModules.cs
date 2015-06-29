using System;
using System.Collections.Generic;
using System.Linq;
using AssemblyReloader.Properties;
using AssemblyReloader.Queries;
using ReeperCommon.Logging;

namespace AssemblyReloader.Game.Providers
{
    public class GetProtoScenarioModules : IGetProtoScenarioModules
    {
        private readonly IKspFactory _kspFactory;
        private readonly IGetTypeIdentifier _getTypeIdentifier;
        private readonly IGetCurrentGame _getCurrentGame;
        private readonly IGetCurrentGameScene _getCurrentGameScene;

        public GetProtoScenarioModules(
            [NotNull] IKspFactory kspFactory,
            [NotNull] IGetTypeIdentifier getTypeIdentifier,
            [NotNull] IGetCurrentGame getCurrentGame,
            [NotNull] IGetCurrentGameScene getCurrentGameScene)
        {
            if (kspFactory == null) throw new ArgumentNullException("kspFactory");
            if (getTypeIdentifier == null) throw new ArgumentNullException("getTypeIdentifier");
            if (getCurrentGame == null) throw new ArgumentNullException("getCurrentGame");
            if (getCurrentGameScene == null) throw new ArgumentNullException("getCurrentGameScene");

            _kspFactory = kspFactory;
            _getTypeIdentifier = getTypeIdentifier;
            _getCurrentGame = getCurrentGame;
            _getCurrentGameScene = getCurrentGameScene;
        }


        public IEnumerable<IProtoScenarioModule> Get([NotNull] Type type)
        {
            if (type == null) throw new ArgumentNullException("type");
            if (!_getCurrentGame.Get().Any()) return new IProtoScenarioModule[0];

            var tempLog = new DebugLog("GetProtoScenarioModules");


            return HighLogic.CurrentGame.scenarios
                .Where(psm => psm != null)
                .Where(psm => psm.moduleName == _getTypeIdentifier.Get(type).Identifier)
                .Where(psm => psm.targetScenes.Contains(_getCurrentGameScene.Get()))
                .Select(psm => _kspFactory.Create(psm));
        }
    }
}
