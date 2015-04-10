using System;
using System.Collections.Generic;
using System.Linq;
using AssemblyReloader.Annotations;
using AssemblyReloader.Queries;

namespace AssemblyReloader.Game.Providers
{
    public class ProtoScenarioModuleProvider : IProtoScenarioModuleProvider
    {
        private readonly IKspFactory _kspFactory;
        private readonly ITypeIdentifierQuery _typeIdentifierQuery;
        private readonly ICurrentGameProvider _currentGameProvider;

        public ProtoScenarioModuleProvider(
            [NotNull] IKspFactory kspFactory,
            [NotNull] ITypeIdentifierQuery typeIdentifierQuery,
            [NotNull] ICurrentGameProvider currentGameProvider)
        {
            if (kspFactory == null) throw new ArgumentNullException("kspFactory");
            if (typeIdentifierQuery == null) throw new ArgumentNullException("typeIdentifierQuery");
            if (currentGameProvider == null) throw new ArgumentNullException("currentGameProvider");

            _kspFactory = kspFactory;
            _typeIdentifierQuery = typeIdentifierQuery;
            _currentGameProvider = currentGameProvider;
        }


        public IEnumerable<IProtoScenarioModule> Get([NotNull] Type type)
        {
            if (type == null) throw new ArgumentNullException("type");
            if (!_currentGameProvider.Get().Any()) return new IProtoScenarioModule[0];

            return HighLogic.CurrentGame.scenarios
                .Where(psm => psm != null)
                .Where(psm => psm.moduleName == _typeIdentifierQuery.Get(type).Identifier)
                .Select(psm => _kspFactory.Create(psm));
        }
    }
}
