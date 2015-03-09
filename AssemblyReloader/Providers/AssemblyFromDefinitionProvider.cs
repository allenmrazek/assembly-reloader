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
    public class AssemblyFromDefinitionProvider : IAssemblyProvider
    {
        private readonly IAssemblyDefinitionReader _reader;
        private readonly ICommand<AssemblyDefinition> _assemblyModifications;

        public AssemblyFromDefinitionProvider(
            IAssemblyDefinitionReader reader,
            ICommand<AssemblyDefinition> assemblyModifications)
        {
            if (reader == null) throw new ArgumentNullException("reader");
            if (assemblyModifications == null) throw new ArgumentNullException("assemblyModifications");

            _reader = reader;
            _assemblyModifications = assemblyModifications;
        }


        public Maybe<Assembly> Get()
        {
            var assemblyDefinition = _reader.GetDefinition();

            if (!assemblyDefinition.Any())
                throw new Exception("Failed to read " + _reader.Name + " definition");

            var def = assemblyDefinition.Single();

            _assemblyModifications.Execute(def);

            using (var stream =new MemoryStream(1024 * 1024))
            {
                _reader.WriteToStream(def, stream);

                return _reader.Load(stream);
            }
        }

        public string Name
        {
            get { return _reader.Name; }
        }
    }
}
