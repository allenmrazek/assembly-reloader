using System.Collections.Generic;
using Mono.Cecil;

namespace AssemblyReloader.ReloadablePlugin.Definition
{
    public class GetAllMethodDefinitions : IGetMethodDefinitions
    {
        public IEnumerable<MethodDefinition> Get(TypeDefinition definition)
        {
            return definition.Methods;
        }
    }
}
