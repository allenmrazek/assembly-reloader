using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using AssemblyReloader.CompositeRoot.Commands;
using AssemblyReloader.DataObjects;
using AssemblyReloader.Loaders;
using AssemblyReloader.Queries.CecilQueries;
using Mono.Cecil;
using ReeperCommon.Containers;

namespace AssemblyReloader.Providers
{
    public class ProxyAssemblyProvider : IAssemblyProvider<ITypeIdentifier>
    {
        private readonly IAssemblyDefinitionReader _reader;
        private readonly IAssemblyDefinitionLoader _loader;
        private readonly ITypeDefinitionQuery _targetTypeQuery;
        private readonly ICommand<AssemblyDefinition> _edits;
        private readonly Dictionary<string, Assembly> _cachedAssemblies = new Dictionary<string, Assembly>();
 
        public ProxyAssemblyProvider(
            IAssemblyDefinitionReader reader,
            IAssemblyDefinitionLoader loader,
            ITypeDefinitionQuery targetTypeQuery,
            ICommand<AssemblyDefinition> edits)
        {
            if (reader == null) throw new ArgumentNullException("reader");
            if (loader == null) throw new ArgumentNullException("loader");
            if (targetTypeQuery == null) throw new ArgumentNullException("targetTypeQuery");
            if (edits == null) throw new ArgumentNullException("edits");

            _reader = reader;
            _loader = loader;
            _targetTypeQuery = targetTypeQuery;
            _edits = edits;
        }


        public Maybe<Assembly> Get(ITypeIdentifier context)
        {
            var cached = RetrieveFromCache(context);
            if (cached.Any())
                return cached;

            var definition = _reader.Get();

            if (!definition.Any())
                throw new InvalidOperationException("Failed to read assembly definition of " + _reader.Name);

            _edits.Execute(definition.Single());
            
            var targets = _targetTypeQuery.Get(definition.Single()).Where(td => td.Name == context.Identifier).ToList();

            if (targets.Count != 1)
                throw new Exception("Expected 1 target type definition in " + _reader.Name + "; found " +
                                    targets.Count);

            targets.Single().Name = context.Identifier;

            var result =  _loader.Load(definition.Single());

            if (result.Any())
                _cachedAssemblies.Add(context.Identifier, result.Single());

            return result;
        }

        public string Name
        {
            get { return _reader.Name; }
        }


        // note: no sense in making duplicate copies of the same thing, so keep track of those
        // already created
        private Maybe<Assembly> RetrieveFromCache(ITypeIdentifier context)
        {
            Assembly result;

            return _cachedAssemblies.TryGetValue(context.Identifier, out result)
                ? Maybe<Assembly>.With(result)
                : Maybe<Assembly>.None;
        }
    }
}
