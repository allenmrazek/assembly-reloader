using System.Collections.Generic;
using System.Linq;
using AssemblyReloader.ReloadablePlugin.Weaving.Operations;
using Mono.Cecil;
using strange.extensions.injector.api;

namespace AssemblyReloader.ReloadablePlugin.Weaving
{
    [Implements(typeof(IGetAllTypesInAssemblyDefinition), InjectionBindingScope.CROSS_CONTEXT)]
    public class GetAllTypesInAssemblyDefinition : IGetAllTypesInAssemblyDefinition
    {
        public IEnumerable<TypeDefinition> Get(AssemblyDefinition definition)
        {
            return definition.Modules
                .SelectMany(module => module.Types)
                .SelectMany(GetNestedTypes);
        }

        private static IEnumerable<TypeDefinition> GetNestedTypes(TypeDefinition type)
        {
            return new[] {type}.Union(type.NestedTypes.SelectMany(GetNestedTypes));
        }

    }
}
