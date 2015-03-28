using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AssemblyReloader.Annotations;
using AssemblyReloader.Game;
using AssemblyReloader.Queries;

namespace AssemblyReloader.Providers.Game
{
    public class ProtoScenarioModuleProvider : IProtoScenarioModuleProvider
    {
        private readonly IKspFactory _kspFactory;
        private readonly ITypeIdentifierQuery _typeIdentifierQuery;

        public ProtoScenarioModuleProvider(
            [NotNull] IKspFactory kspFactory,
            [NotNull] ITypeIdentifierQuery typeIdentifierQuery)
        {
            if (kspFactory == null) throw new ArgumentNullException("kspFactory");
            if (typeIdentifierQuery == null) throw new ArgumentNullException("typeIdentifierQuery");

            _kspFactory = kspFactory;
            _typeIdentifierQuery = typeIdentifierQuery;
        }


        public IEnumerable<IProtoScenarioModule> Get(Type type)
        {
            return HighLogic.CurrentGame.scenarios
                .Where(psm => psm.moduleName == _typeIdentifierQuery.Get(type).Identifier)
                .Select(psm => _kspFactory.Create(psm));
        }
    }
}
