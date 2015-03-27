using System.Collections.Generic;
using Mono.Cecil;

namespace AssemblyReloader.Queries.CecilQueries
{
    public interface IMethodDefinitionQuery
    {
        IEnumerable<MethodDefinition> Get(TypeDefinition definition);
    }
}
