extern alias Cecil96;
using System.Collections.Generic;
using AssemblyDefinition = Cecil96::Mono.Cecil.AssemblyDefinition;
using TypeDefinition = Cecil96::Mono.Cecil.TypeDefinition;

namespace AssemblyReloader.ReloadablePlugin.Weaving.Operations
{
    public interface IGetAllTypesInAssemblyDefinition
    {
        IEnumerable<TypeDefinition> Get(AssemblyDefinition definition);
    }
}
