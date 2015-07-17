using System;
using System.Collections.Generic;
using System.Linq;
using AssemblyReloader.Queries.CecilQueries;
using AssemblyReloader.ReloadablePlugin.Weaving.old.Definition;
using Mono.Cecil;

namespace AssemblyReloader.Queries
{
    public class GetInjectedHelperTypeMethod : IGetMethodDefinitions
    {
        private readonly IGetTypeDefinitions _injectedGetTypeQuery;
        private readonly string _methodName;

        public GetInjectedHelperTypeMethod(IGetTypeDefinitions injectedGetTypeQuery, string methodName)
        {
            if (injectedGetTypeQuery == null) throw new ArgumentNullException("injectedGetTypeQuery");
            if (string.IsNullOrEmpty(methodName)) throw new ArgumentNullException("methodName");

            _injectedGetTypeQuery = injectedGetTypeQuery;
            _methodName = methodName;
        }


        public IEnumerable<MethodDefinition> Get(TypeDefinition definition)
        {
            var injectedType = _injectedGetTypeQuery.Get(definition.Module.Assembly).ToList();

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
