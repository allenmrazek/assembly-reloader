using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;

namespace AssemblyReloader.Queries.CecilQueries
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


        public IEnumerable<MethodDefinition> Get(AssemblyDefinition definition)
        {
            var injectedType = _injectedTypeQuery.Get(definition).ToList();

            if (!injectedType.Any())
                throw new Exception("Failed to locate injected helper type in " + definition.FullName);

            if (injectedType.Count > 1)
                throw new Exception("Too many injected type definitions found (expected 1, found " + injectedType.Count +
                                    ")");

            return injectedType.Single().Methods
                .Where(md => md.Name == _methodName);
        }
    }
}
