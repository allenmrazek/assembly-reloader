using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using ReeperCommon.Logging.Implementations;

namespace AssemblyReloader.Queries.CecilQueries.Injected
{
    public class InjectedHelperTypeMethodQuery : IMethodDefinitionQuery
    {
        private readonly ITypeDefinitionQuery _injectedTypeQuery;
        private readonly string _methodName;

        public InjectedHelperTypeMethodQuery(ITypeDefinitionQuery injectedTypeQuery, string methodName)
        {
            if (injectedTypeQuery == null) throw new ArgumentNullException("injectedTypeQuery");
            if (string.IsNullOrEmpty(methodName)) throw new ArgumentNullException("methodName");

            _injectedTypeQuery = injectedTypeQuery;
            _methodName = methodName;
        }


        public IEnumerable<MethodDefinition> Get(TypeDefinition definition)
        {
            var injectedType = _injectedTypeQuery.Get(definition.Module.Assembly).ToList();

            if (!injectedType.Any())
                throw new Exception("Failed to locate injected helper type in " + definition.FullName);

            if (injectedType.Count > 1)
                throw new Exception("Too many injected type definitions found (expected 1, found " + injectedType.Count +
                                    ")");

            var log = new DebugLog("InjectedHelperTypeMethodQuery");
            injectedType.ToDictionary(td => td, td => td.Methods).ToList().ForEach(kvp =>
            {
                log.Normal("Type: " + kvp.Key.FullName);
                foreach (var m in kvp.Value) log.Normal("  Method: " + m.FullName);
            });

            return injectedType.Single().Methods
                .Where(md => md.Name == _methodName);
        }
    }
}
