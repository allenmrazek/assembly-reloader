using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;

namespace AssemblyReloader.ReloadablePlugin.Loaders.Definition
{
    public class GetAllTypesFromDefinition : IGetTypeDefinitions
    {
        public IEnumerable<TypeDefinition> Get(AssemblyDefinition assembly)
        {
            return assembly.Modules.SelectMany(md => md.Types);
        }
    }
}
