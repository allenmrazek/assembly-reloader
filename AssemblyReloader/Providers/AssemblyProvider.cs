using System;
using System.IO;
using System.Linq;
using System.Reflection;
using AssemblyReloader.CompositeRoot.Commands;
using AssemblyReloader.Loaders;
using Mono.Cecil;
using ReeperCommon.Containers;
using ReeperCommon.FileSystem;

namespace AssemblyReloader.Providers
{
    public class AssemblyProvider : IAssemblyProvider
    {
        private readonly IAssemblyDefinitionReader _reader;
        private readonly IAssemblyDefinitionLoader _loader;
        private readonly ICommand<AssemblyDefinition> _assemblyModifications;

        public AssemblyProvider(
            IAssemblyDefinitionReader reader,
            IAssemblyDefinitionLoader loader,
            ICommand<AssemblyDefinition> assemblyModifications)
        {
            if (reader == null) throw new ArgumentNullException("reader");
            if (loader == null) throw new ArgumentNullException("loader");
            if (assemblyModifications == null) throw new ArgumentNullException("assemblyModifications");
            _reader = reader;
            _loader = loader;
            _assemblyModifications = assemblyModifications;
        }


        public Maybe<Assembly> Get()
        {
            var assemblyDefinition = _reader.Get();

            if (!assemblyDefinition.Any())
                throw new Exception("Failed to read " + _reader.Name + " definition");

            var def = assemblyDefinition.Single();

            _assemblyModifications.Execute(def);


            return _loader.Load(def);
        }

        public string Name
        {
            get { return _reader.Name; }
        }
    }
}
