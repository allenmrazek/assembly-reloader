using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;

namespace AssemblyReloader.Queries.CecilQueries
{
    public class ExcludingMethodDefinitions : IMethodDefinitionQuery
    {
        private readonly IMethodDefinitionQuery _queryToExcludeFrom;
        private readonly IMethodDefinitionQuery _definitionsToExcludeQuery;

        public ExcludingMethodDefinitions(IMethodDefinitionQuery queryToExcludeFrom, IMethodDefinitionQuery definitionsToExcludeQuery)
        {
            if (queryToExcludeFrom == null) throw new ArgumentNullException("queryToExcludeFrom");
            if (definitionsToExcludeQuery == null) throw new ArgumentNullException("definitionsToExcludeQuery");

            _queryToExcludeFrom = queryToExcludeFrom;
            _definitionsToExcludeQuery = definitionsToExcludeQuery;
        }


        public IEnumerable<MethodDefinition> Get(TypeDefinition definition)
        {
            return _queryToExcludeFrom.Get(definition).Except(_definitionsToExcludeQuery.Get(definition));
        }
    }
}
