using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;

namespace AssemblyReloader.ReloadablePlugin.Weaving.Operations
{
    public class GetTypeDefinitionsInAssemblyDefinitionExcludingHelper : IGetTypeDefinitions
    {
        public IEnumerable<TypeDefinition> Get(AssemblyDefinition assembly)
        {
            return assembly.Modules.SelectMany(md => md.Types)
                .Where(td => td.Namespace != CommandInsertHelperType.Namespace && td.Name != CommandInsertHelperType.TypeName);
        }
    }
}
