using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;

namespace AssemblyReloader.Queries.CecilQueries
{
    public class PartModuleDefinitionsQuery : IPartModuleDefinitionsQuery
    {
        public IEnumerable<TypeDefinition> Get(AssemblyDefinition assembly)
        {
            if (assembly == null) throw new ArgumentNullException("assembly");

            return assembly.MainModule.Types.Where(IsPartModule);
        }

        private bool IsPartModule(TypeDefinition type)
        {
            if (type.BaseType == null) return false;

            if (type.BaseType.Name == typeof (PartModule).Name)
                return true;

            return type.BaseType != null && IsPartModule(type.Resolve().BaseType.Resolve());
        }
    }
}
