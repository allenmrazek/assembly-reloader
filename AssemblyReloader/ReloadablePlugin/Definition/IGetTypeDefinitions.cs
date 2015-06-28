using System.Collections.Generic;
using Mono.Cecil;

namespace AssemblyReloader.ReloadablePlugin.Definition
{
    public interface IGetTypeDefinitions
    {
        IEnumerable<TypeDefinition> Get(AssemblyDefinition assembly);
    }
}
