extern alias Cecil96;
using System.Collections.Generic;
using System.Linq;
using AssemblyDefinition = Cecil96::Mono.Cecil.AssemblyDefinition;
using TypeDefinition = Cecil96::Mono.Cecil.TypeDefinition;

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
