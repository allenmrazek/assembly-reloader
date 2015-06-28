using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;

namespace AssemblyReloader.ReloadablePlugin.Definition
{
    public class GetTypeDefinitionsExcluding : IGetTypeDefinitions
    {
        private readonly IGetTypeDefinitions _queryToExcludeFrom;
        private readonly IGetTypeDefinitions _injectedGetTypeQuery;

        public GetTypeDefinitionsExcluding(IGetTypeDefinitions queryToExcludeFrom, IGetTypeDefinitions injectedGetTypeQuery)
        {
            if (queryToExcludeFrom == null) throw new ArgumentNullException("queryToExcludeFrom");
            if (injectedGetTypeQuery == null) throw new ArgumentNullException("injectedGetTypeQuery");

            _queryToExcludeFrom = queryToExcludeFrom;
            _injectedGetTypeQuery = injectedGetTypeQuery;
        }


        public IEnumerable<TypeDefinition> Get(AssemblyDefinition assembly)
        {
            return _queryToExcludeFrom.Get(assembly).Except(_injectedGetTypeQuery.Get(assembly));
        }
    }
}
