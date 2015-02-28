using System.Collections.Generic;
using Mono.Cecil;

namespace AssemblyReloader.Queries.CecilQueries
{
    public interface ITypeDefinitionQuery
    {
        IEnumerable<TypeDefinition> Get(AssemblyDefinition assembly);
    }
}
