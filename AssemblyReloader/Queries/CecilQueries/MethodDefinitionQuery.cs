using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;

namespace AssemblyReloader.Queries.CecilQueries
{
    public class MethodDefinitionQuery : IMethodDefinitionQuery
    {
        public IEnumerable<MethodDefinition> Get(TypeDefinition typeDefinition, string methodName)
        {
            return typeDefinition.Methods
                .Where(methodDef => methodDef.Name == methodName)
                .Where(methodDef => methodDef.IsVirtual)
                .Where(methodDef => methodDef.Parameters.First().ParameterType.FullName == typeof (ConfigNode).FullName)
                .Where(methodDef => methodDef.Parameters.Count == 1);

        }
    }
}
