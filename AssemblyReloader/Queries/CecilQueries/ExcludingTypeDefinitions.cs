using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;

namespace AssemblyReloader.Queries.CecilQueries
{
    public class ExcludingTypeDefinitions : ITypeDefinitionQuery
    {
        private readonly ITypeDefinitionQuery _queryToExcludeFrom;
        private readonly ITypeDefinitionQuery _injectedTypeQuery;

        public ExcludingTypeDefinitions(ITypeDefinitionQuery queryToExcludeFrom, ITypeDefinitionQuery injectedTypeQuery)
        {
            if (queryToExcludeFrom == null) throw new ArgumentNullException("queryToExcludeFrom");
            if (injectedTypeQuery == null) throw new ArgumentNullException("injectedTypeQuery");

            _queryToExcludeFrom = queryToExcludeFrom;
            _injectedTypeQuery = injectedTypeQuery;
        }


        public IEnumerable<TypeDefinition> Get(AssemblyDefinition assembly)
        {
            return _queryToExcludeFrom.Get(assembly).Except(_injectedTypeQuery.Get(assembly));
        }
    }
}
