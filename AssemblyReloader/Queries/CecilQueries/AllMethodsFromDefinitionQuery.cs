using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;

namespace AssemblyReloader.Queries.CecilQueries
{
    public class AllMethodsFromDefinitionQuery : IMethodDefinitionQuery
    {
        public IEnumerable<MethodDefinition> Get(AssemblyDefinition definition)
        {
            return definition.Modules
                .SelectMany(module => module.GetTypes()
                    .SelectMany(td => td.Methods));
        }
    }
}
