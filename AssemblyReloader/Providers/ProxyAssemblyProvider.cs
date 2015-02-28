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
        private readonly IPartModuleDefinitionsQuery _pmQuery;
        private readonly ICommand<AssemblyDefinition> _edits;
        private readonly Dictionary<string, Assembly> _cachedAssemblies = new Dictionary<string, Assembly>();
 
        public ProxyAssemblyProvider(
            IAssemblyDefinitionReader reader,
            IAssemblyDefinitionLoader loader,
            IPartModuleDefinitionsQuery pmQuery,
            ICommand<AssemblyDefinition> edits)
        {
            if (reader == null) throw new ArgumentNullException("reader");
            if (loader == null) throw new ArgumentNullException("loader");
            if (pmQuery == null) throw new ArgumentNullException("pmQuery");
            if (edits == null) throw new ArgumentNullException("edits");

            _reader = reader;
            _loader = loader;
            _pmQuery = pmQuery;
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
            
            var partModules = _pmQuery.Get(definition.Single()).Where(td => td.Name == context.Identifier).ToList();

            if (partModules.Count != 1)
                throw new Exception("Expected 1 PartModule definition in " + _reader.Name + "; found " +
                                    partModules.Count);

            partModules.Single().Name = context.Identifier;

            return _loader.Load(definition.Single());
        }


        private Maybe<Assembly> RetrieveFromCache(ITypeIdentifier context)
        {
            Assembly result;

            return _cachedAssemblies.TryGetValue(context.Identifier, out result)
                ? Maybe<Assembly>.With(result)
                : Maybe<Assembly>.None;
        }
    }
}
