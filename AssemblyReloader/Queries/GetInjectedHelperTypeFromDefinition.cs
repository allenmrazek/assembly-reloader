using System.Collections.Generic;
using System.Linq;
using AssemblyReloader.ReloadablePlugin.Weaving.Commands;
using AssemblyReloader.ReloadablePlugin.Weaving.old.Definition;
using AssemblyReloader.ReloadablePlugin.Weaving.old.Definition.Operations.old;
using Mono.Cecil;

namespace AssemblyReloader.Queries
{
    public class GetInjectedHelperTypeFromDefinition : IGetTypeDefinitions
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
