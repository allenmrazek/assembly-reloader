using System.Collections.Generic;
using Mono.Cecil;

namespace AssemblyReloader.ReloadablePlugin.Weaving.old.Definition
{
    public interface IGetTypeDefinitions
    {
        IEnumerable<TypeDefinition> Get(AssemblyDefinition assembly);
    }
}
