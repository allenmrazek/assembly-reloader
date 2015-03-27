using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AssemblyReloader.Annotations;
using AssemblyReloader.Queries;

namespace AssemblyReloader.Providers.Game
{
    public class ProtoScenarioModuleProvider : IProtoScenarioModuleProvider
    {
        private readonly ITypeIdentifierQuery _typeIdentifierQuery;

        public ProtoScenarioModuleProvider([NotNull] ITypeIdentifierQuery typeIdentifierQuery)
        {
            if (typeIdentifierQuery == null) throw new ArgumentNullException("typeIdentifierQuery");

            _typeIdentifierQuery = typeIdentifierQuery;
        }


        public IEnumerable<ProtoScenarioModule> Get(Type type)
        {
            return HighLogic.CurrentGame.scenarios.Where(psm => psm.moduleName == _typeIdentifierQuery.Get(type).Identifier);
        }
    }
}
