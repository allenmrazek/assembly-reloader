﻿using System.Collections.Generic;
using System.Linq;
using AssemblyReloader.Weaving.Operations;
using Mono.Cecil;

namespace AssemblyReloader.Queries.CecilQueries.Injected
{
    public class InjectedHelperTypeQuery : ITypeDefinitionQuery
    {
        public IEnumerable<TypeDefinition> Get(AssemblyDefinition assembly)
        {
            return
                assembly.Modules
                    .SelectMany(m => m.Types)
                    .Where(t => t.Namespace == InjectedHelperTypeDefinitionWriter.Namespace)
                    .Where(t => t.Name == InjectedHelperTypeDefinitionWriter.TypeName);
        }
    }
}