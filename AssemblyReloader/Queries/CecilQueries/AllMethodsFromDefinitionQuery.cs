using System.Collections.Generic;
using Mono.Cecil;

namespace AssemblyReloader.Queries.CecilQueries
{
    public class AllMethodsFromDefinitionQuery : IMethodDefinitionQuery
    {
        public IEnumerable<MethodDefinition> Get(TypeDefinition definition)
        {
            return definition.Methods;
        }
    }
}
