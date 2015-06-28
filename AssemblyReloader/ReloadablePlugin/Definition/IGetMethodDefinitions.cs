using System.Collections.Generic;
using Mono.Cecil;

namespace AssemblyReloader.ReloadablePlugin.Definition
{
    public interface IGetMethodDefinitions
    {
        IEnumerable<MethodDefinition> Get(TypeDefinition definition);
    }
}
