using System;
using System.IO;
using Mono.Cecil;

namespace AssemblyReloader.Loaders
{
    public abstract class AssemblyDefinitionLoaderBase
    {
        protected virtual MemoryStream WriteDefinitionToStream(AssemblyDefinition definition)
        {
            if (definition == null) throw new ArgumentNullException("definition");

            var stream = new MemoryStream();

            definition.Write(stream);

            return stream;
        }
    }
}
