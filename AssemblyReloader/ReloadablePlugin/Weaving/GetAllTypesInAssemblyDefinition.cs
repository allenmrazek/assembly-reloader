extern alias Cecil96;
using System.Collections.Generic;
using System.Linq;
using AssemblyReloader.ReloadablePlugin.Weaving.Operations;
using strange.extensions.implicitBind;
using strange.extensions.injector.api;
using AssemblyDefinition = Cecil96::Mono.Cecil.AssemblyDefinition;
using TypeDefinition = Cecil96::Mono.Cecil.TypeDefinition;

namespace AssemblyReloader.ReloadablePlugin.Weaving
{
    // note: this will return ALL defined types, including any we defined ourselves (like helper type).
    // You should probably use IGetTypeDefinitions
    [Implements(typeof(IGetAllTypesInAssemblyDefinition), InjectionBindingScope.CROSS_CONTEXT)]
// ReSharper disable once ClassNeverInstantiated.Global
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
