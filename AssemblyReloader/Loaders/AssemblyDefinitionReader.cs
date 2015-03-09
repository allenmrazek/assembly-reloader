using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Mono.Cecil;
using ReeperCommon.Containers;
using ReeperCommon.Extensions;
using ReeperCommon.FileSystem;

namespace AssemblyReloader.Loaders
{
    public class AssemblyDefinitionReader : IAssemblyDefinitionReader
    {
        private readonly BaseAssemblyResolver _resolver;

        public AssemblyDefinitionReader(
            IFile location, 
            BaseAssemblyResolver resolver)
        {
            Location = location;
            if (location == null) throw new ArgumentNullException("location");
            if (resolver == null) throw new ArgumentNullException("resolver");

            _resolver = resolver;
        }


        public Maybe<AssemblyDefinition> GetDefinition()
        {
            var definition = AssemblyDefinition.ReadAssembly(Location.FullPath, new ReaderParameters
            {
                AssemblyResolver = _resolver,
            });

            return definition.IsNull() ? Maybe<AssemblyDefinition>.None : Maybe<AssemblyDefinition>.With(definition);
        }

        public void WriteToStream(AssemblyDefinition definition, Stream stream)
        {
            if (definition == null) throw new ArgumentNullException("definition");
            if (stream == null) throw new ArgumentNullException("stream");

            definition.Write(stream);

        }

        public Maybe<Assembly> Load(MemoryStream stream)
        {
            if (stream == null) throw new ArgumentNullException("stream");

            if (stream.Length == 0) throw new ArgumentException("stream does not contain any data");

            var result = Assembly.Load(stream.GetBuffer());

            return result.IsNull() ? Maybe<Assembly>.None : Maybe<Assembly>.With(result);
            
        }

        public string Name
        {
            get { return Location.Name; }
        }

        public IFile Location { get; private set; }
    }
}
