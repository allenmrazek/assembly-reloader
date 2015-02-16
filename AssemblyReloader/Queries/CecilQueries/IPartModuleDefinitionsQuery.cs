using System.Collections.Generic;
using Mono.Cecil;

namespace AssemblyReloader.Queries.CecilQueries
{
    public interface IPartModuleDefinitionsQuery
    {
        IEnumerable<TypeDefinition> Get(AssemblyDefinition assembly);
    }
}
