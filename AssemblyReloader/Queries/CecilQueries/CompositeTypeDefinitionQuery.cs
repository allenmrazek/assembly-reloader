using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;

namespace AssemblyReloader.Queries.CecilQueries
{
    public class CompositeTypeDefinitionQuery : ITypeDefinitionQuery
    {
        private readonly ITypeDefinitionQuery[] _queries;

        public CompositeTypeDefinitionQuery(params ITypeDefinitionQuery[] queries)
        {
            if (queries == null) throw new ArgumentNullException("queries");
            _queries = queries;
        }


        public IEnumerable<TypeDefinition> Get(AssemblyDefinition assembly)
        {
            return _queries.SelectMany(q => q.Get(assembly));
        }
    }
}
