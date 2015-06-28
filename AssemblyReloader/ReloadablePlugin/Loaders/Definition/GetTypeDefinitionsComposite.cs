using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;

namespace AssemblyReloader.ReloadablePlugin.Loaders.Definition
{
    public class GetTypeDefinitionsComposite : IGetTypeDefinitions
    {
        private readonly IGetTypeDefinitions[] _queries;

        public GetTypeDefinitionsComposite(params IGetTypeDefinitions[] queries)
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
