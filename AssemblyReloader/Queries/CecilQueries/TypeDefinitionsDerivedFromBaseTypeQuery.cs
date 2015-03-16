using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;

namespace AssemblyReloader.Queries.CecilQueries
{
    public class TypeDefinitionsDerivedFromBaseTypeQuery<T> : ITypeDefinitionQuery
    {
        private readonly ITypeDefinitionQuery _allTypesFromDefinitionQuery;

        public TypeDefinitionsDerivedFromBaseTypeQuery(ITypeDefinitionQuery allTypesFromDefinitionQuery)
        {
            if (allTypesFromDefinitionQuery == null) throw new ArgumentNullException("allTypesFromDefinitionQuery");
            _allTypesFromDefinitionQuery = allTypesFromDefinitionQuery;
        }


        public IEnumerable<TypeDefinition> Get(AssemblyDefinition assembly)
        {
            return _allTypesFromDefinitionQuery.Get(assembly)
                .Where(td => IsDerivedFrom(td, typeof (T)));
        }

        private bool IsDerivedFrom(TypeDefinition definition, Type baseType)
        {
            if (definition.BaseType == null) return false;

            if (definition.BaseType.Name == baseType.Name)
                return true;

            return definition.BaseType != null && IsDerivedFrom(definition.BaseType.Resolve(), baseType);
        }
    }
}
