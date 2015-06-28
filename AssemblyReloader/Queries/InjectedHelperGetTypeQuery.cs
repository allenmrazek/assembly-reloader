using System.Collections.Generic;
using System.Linq;
using AssemblyReloader.Queries.CecilQueries;
using AssemblyReloader.ReloadablePlugin.Definition;
using AssemblyReloader.ReloadablePlugin.Definition.Operations;
using AssemblyReloader.ReloadablePlugin.Definition.Operations.old;
using Mono.Cecil;

namespace AssemblyReloader.Queries
{
    public class InjectedHelperGetTypeQuery : IGetTypeDefinitions
    {
        public IEnumerable<TypeDefinition> Get(AssemblyDefinition assembly)
        {
            return
                assembly.Modules
                    .SelectMany(m => m.Types)
                    .Where(t => t.Namespace == InjectedHelperTypeDefinitionWriter.Namespace)
                    .Where(t => t.Name == InjectedHelperTypeDefinitionWriter.TypeName);
        }
    }
}
