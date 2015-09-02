extern alias Cecil96;
using System;
using System.Collections.Generic;
using System.Linq;
using AssemblyDefinition = Cecil96::Mono.Cecil.AssemblyDefinition;
using TypeDefinition = Cecil96::Mono.Cecil.TypeDefinition;

namespace AssemblyReloader.ReloadablePlugin.Weaving.Operations
{
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
