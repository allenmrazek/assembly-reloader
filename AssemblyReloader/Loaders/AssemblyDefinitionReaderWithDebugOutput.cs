using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Mono.Cecil;
using ReeperCommon.Containers;
using ReeperCommon.FileSystem;

namespace AssemblyReloader.Loaders
{
    public class AssemblyDefinitionReaderWithDebugOutput : IAssemblyDefinitionReader
    {
        private readonly IAssemblyDefinitionReader _reader;

        public AssemblyDefinitionReaderWithDebugOutput(IAssemblyDefinitionReader reader)
        {
            if (reader == null) throw new ArgumentNullException("reader");
            _reader = reader;
        }


        public Maybe<AssemblyDefinition> GetDefinition()
        {
            return _reader.GetDefinition();
        }

        public void WriteToStream(AssemblyDefinition definition, Stream stream)
        {
            var filename = _reader.Location.FullPath + ".debug";

            if (File.Exists(filename))
                File.Delete(filename);

            definition.Write(filename);

            _reader.WriteToStream(definition, stream);
        }

        public Maybe<Assembly> Load(MemoryStream stream)
        {
            return _reader.Load(stream);
        }

        public string Name
        {
            get { return _reader.Name; }
        }

        public IFile Location
        {
            get { return _reader.Location; }
        }
    }
}
