using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using AssemblyReloader.Annotations;
using AssemblyReloader.Loaders;
using AssemblyReloader.Weaving;
using FinePrint.Contracts.Parameters;
using Mono.Cecil;
using ReeperCommon.Containers;
using ReeperCommon.FileSystem;

namespace AssemblyReloader.Providers
{
    public class AssemblyProvider : IAssemblyProvider
    {
        private readonly IAssemblyDefinitionReader _reader;
        private readonly IAssemblyDefinitionMemoryLoader _memoryLoader;
        private readonly IAssemblyDefinitionWeaver _weaver;

        public AssemblyProvider(
            [NotNull] IAssemblyDefinitionReader reader,
            [NotNull] IAssemblyDefinitionMemoryLoader memoryLoader, 
            [NotNull] IAssemblyDefinitionWeaver weaver)
        {
            if (reader == null) throw new ArgumentNullException("reader");
            if (memoryLoader == null) throw new ArgumentNullException("memoryLoader");
            if (weaver == null) throw new ArgumentNullException("weaver");

            _reader = reader;
            _memoryLoader = memoryLoader;
            _weaver = weaver;
        }


        public Maybe<Assembly> Get()
        {
            var definition = ReadAssemblyDefinition();

            if (!_weaver.Weave(definition))
                throw new Exception("Failed to reweave definition of " + Location.FileName);

            return _memoryLoader.LoadDefinition(definition);
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



            //var assemblyDefinition = _definitionReader.Get();

            //if (!assemblyDefinition.Any())
            //    throw new Exception("Failed to read " + _definitionReader.Name + " definition");

            

            //if (!_weaver.Weave(assemblyDefinition.Single()))
            //    throw new Exception("Failed to reweave " + _definitionReader.Name + " il");

            //// try to write first, in case loading it from memory fails (usually due to an error
            //// in rewritten il code)

            //if (_writeResultToDisk)
            //    assemblyDefinition.Single().Write(_definitionReader.Location.FullPath + ".debug", new WriterParameters { WriteSymbols = true});

            //var result = _assemblyFromDefinitionProvider.Get(assemblyDefinition.Single());

            //if (!result.Any()) return Maybe<Assembly>.None;
