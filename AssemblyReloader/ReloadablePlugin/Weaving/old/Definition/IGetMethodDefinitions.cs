using System.Collections.Generic;
using Mono.Cecil;

namespace AssemblyReloader.ReloadablePlugin.Weaving.old.Definition
{
    public interface IGetMethodDefinitions
    {
        IEnumerable<MethodDefinition> Get(TypeDefinition definition);
    }
}
