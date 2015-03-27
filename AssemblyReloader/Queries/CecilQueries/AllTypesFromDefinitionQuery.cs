using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;

namespace AssemblyReloader.Queries.CecilQueries
{
    public class AllTypesFromDefinitionQuery : ITypeDefinitionQuery
    {
        public IEnumerable<TypeDefinition> Get(AssemblyDefinition assembly)
        {
            return assembly.Modules.SelectMany(md => md.Types);
        }
    }
}
