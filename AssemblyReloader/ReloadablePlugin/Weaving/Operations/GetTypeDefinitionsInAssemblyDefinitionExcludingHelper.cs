extern alias Cecil96;
using System;
using System.Collections.Generic;
using System.Linq;
using strange.extensions.implicitBind;
using strange.extensions.injector.api;
using AssemblyDefinition = Cecil96::Mono.Cecil.AssemblyDefinition;
using TypeDefinition = Cecil96::Mono.Cecil.TypeDefinition;

namespace AssemblyReloader.ReloadablePlugin.Weaving.Operations
{
    [Implements(typeof(IGetTypeDefinitions), InjectionBindingScope.CROSS_CONTEXT)]
// ReSharper disable once ClassNeverInstantiated.Global
    public class GetTypeDefinitionsInAssemblyDefinitionExcludingHelper : IGetTypeDefinitions
    {
        private readonly IGetAllTypesInAssemblyDefinition _allTypesInAssembly;

        public GetTypeDefinitionsInAssemblyDefinitionExcludingHelper(IGetAllTypesInAssemblyDefinition allTypesInAssembly)
        {
            if (allTypesInAssembly == null) throw new ArgumentNullException("allTypesInAssembly");
            _allTypesInAssembly = allTypesInAssembly;
        }


        public IEnumerable<TypeDefinition> Get(AssemblyDefinition assembly)
        {
            return _allTypesInAssembly.Get(assembly)
                .Where(td => td.Namespace != CommandInsertHelperType.Namespace && td.Name != CommandInsertHelperType.TypeName);
        }
    }
}
