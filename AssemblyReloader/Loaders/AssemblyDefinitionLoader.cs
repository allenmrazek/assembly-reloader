using System;
using System.IO;
using System.Reflection;
using Mono.Cecil;
using ReeperCommon.Containers;
using ReeperCommon.Extensions;

namespace AssemblyReloader.Loaders
{
    public class AssemblyDefinitionLoader : IAssemblyDefinitionLoader
    {
        public Maybe<Assembly> Load(AssemblyDefinition definition)
        {
            using (var stream = WriteDefinitionToStream(definition))
            {
                var result = Assembly.Load(stream.GetBuffer());

                return result.IsNull() ? Maybe<Assembly>.None : Maybe<Assembly>.With(result);
            }
        }


        private MemoryStream WriteDefinitionToStream(AssemblyDefinition definition)
        {
            if (definition == null) throw new ArgumentNullException("definition");

            var stream = new MemoryStream();

            definition.Write(stream);

            return stream;
        }
    }
}
