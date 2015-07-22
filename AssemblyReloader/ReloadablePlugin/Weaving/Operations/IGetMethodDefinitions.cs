using System.Collections.Generic;
using Mono.Cecil;

namespace AssemblyReloader.ReloadablePlugin.Weaving.Operations
{
    public interface IGetMethodDefinitions
    {
        IEnumerable<MethodDefinition> Get(TypeDefinition definition);
    }
}
