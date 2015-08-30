extern alias Cecil96;
using System.Collections.Generic;
using TypeDefinition  = Cecil96::Mono.Cecil.TypeDefinition;
using AssemblyDefinition = Cecil96::Mono.Cecil.AssemblyDefinition;

namespace AssemblyReloader.ReloadablePlugin.Weaving.Operations
{
    public interface IGetTypeDefinitions
    {
        IEnumerable<TypeDefinition> Get(AssemblyDefinition assembly);
    }
}
