using System.Collections.Generic;
using Mono.Cecil;

namespace AssemblyReloader.ReloadablePlugin.Weaving.Commands
{
    public interface IGetTypeDefinitions
    {
        IEnumerable<TypeDefinition> Get(AssemblyDefinition assembly);
    }
}
