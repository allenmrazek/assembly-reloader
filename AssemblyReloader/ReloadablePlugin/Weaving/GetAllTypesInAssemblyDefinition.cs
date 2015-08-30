extern alias Cecil96;
using System.Collections.Generic;
using System.Linq;
using AssemblyReloader.ReloadablePlugin.Weaving.Operations;
using strange.extensions.injector.api;
using AssemblyDefinition = Cecil96::Mono.Cecil.AssemblyDefinition;
using TypeDefinition = Cecil96::Mono.Cecil.TypeDefinition;

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
