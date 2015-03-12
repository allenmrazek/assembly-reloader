using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;

namespace AssemblyReloader.Queries.CecilQueries
{
    public interface IMethodDefinitionQuery
    {
        IEnumerable<MethodDefinition> Get(AssemblyDefinition definition);
    }
}
