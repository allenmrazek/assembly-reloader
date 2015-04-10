using System;
using System.IO;
using System.Linq;
using System.Reflection;
using AssemblyReloader.Annotations;
using AssemblyReloader.Loaders;
using AssemblyReloader.Weaving;
using Mono.Cecil;
using ReeperCommon.Containers;
using ReeperCommon.FileSystem;

namespace AssemblyReloader.Providers
{
    public class AssemblyProvider : IAssemblyProvider
    {
        private readonly IAssemblyDefinitionReader _reader;
        private readonly IAssemblyDefinitionLoader _loader;
        private readonly IAssemblyDefinitionWeaver _weaver;

        public AssemblyProvider(
            [NotNull] IAssemblyDefinitionReader reader,
            [NotNull] IAssemblyDefinitionLoader loader, 
            [NotNull] IAssemblyDefinitionWeaver weaver)
        {
            if (reader == null) throw new ArgumentNullException("reader");
            if (loader == null) throw new ArgumentNullException("loader");
            if (weaver == null) throw new ArgumentNullException("weaver");

            _reader = reader;
            _loader = loader;
            _weaver = weaver;
        }


        public Maybe<Assembly> Get()
        {
            var definition = ReadAssemblyDefinition();

            if (!_weaver.Weave(definition))
                throw new Exception("Failed to reweave definition of " + Location.FileName);

            return _loader.LoadDefinition(definition);
        }




        private AssemblyDefinition ReadAssemblyDefinition()
        {
            var definition = _reader.Get();
            if (!definition.Any())
                throw new FileLoadException("Failed to read assembly definition from " + Location.FullPath);

            return definition.Single();
        }




        public IFile Location
        {
            get { return _reader.Location; }
        }
    }
}