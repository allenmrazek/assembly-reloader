using System;
using System.Collections.Generic;
using System.Linq;
using AssemblyReloader.Properties;
using AssemblyReloader.Queries;
using ReeperCommon.Logging;

namespace AssemblyReloader.Game.Providers
{
    public class ProtoScenarioModuleProvider : IProtoScenarioModuleProvider
    {
        private readonly IKspFactory _kspFactory;
        private readonly ITypeIdentifierQuery _typeIdentifierQuery;
        private readonly ICurrentGameProvider _currentGameProvider;
        private readonly ICurrentGameSceneProvider _currentGameSceneProvider;

        public ProtoScenarioModuleProvider(
            [NotNull] IKspFactory kspFactory,
            [NotNull] ITypeIdentifierQuery typeIdentifierQuery,
            [NotNull] ICurrentGameProvider currentGameProvider,
            [NotNull] ICurrentGameSceneProvider currentGameSceneProvider)
        {
            if (kspFactory == null) throw new ArgumentNullException("kspFactory");
            if (typeIdentifierQuery == null) throw new ArgumentNullException("typeIdentifierQuery");
            if (currentGameProvider == null) throw new ArgumentNullException("currentGameProvider");
            if (currentGameSceneProvider == null) throw new ArgumentNullException("currentGameSceneProvider");

            _kspFactory = kspFactory;
            _typeIdentifierQuery = typeIdentifierQuery;
            _currentGameProvider = currentGameProvider;
            _currentGameSceneProvider = currentGameSceneProvider;
        }


        public IEnumerable<IProtoScenarioModule> Get([NotNull] Type type)
        {
            if (type == null) throw new ArgumentNullException("type");
            if (!_currentGameProvider.Get().Any()) return new IProtoScenarioModule[0];

            var tempLog = new DebugLog("ProtoScenarioModuleProvider");


            return HighLogic.CurrentGame.scenarios
                .Where(psm => psm != null)
                .Where(psm => psm.moduleName == _typeIdentifierQuery.Get(type).Identifier)
                .Where(psm => psm.targetScenes.Contains(_currentGameSceneProvider.Get()))
                .Select(psm => _kspFactory.Create(psm));
        }
    }
}
