using System;
using System.IO;
using System.Reflection;
using AssemblyReloader.Disk;
using Mono.Cecil;
using ReeperCommon.Containers;

namespace AssemblyReloader.Providers
{
    public class AssemblyFromDefinitionProvider : IAssemblyProvider
    {
        public Maybe<Assembly> Get(AssemblyDefinition definition)
        {
            if (definition == null) throw new ArgumentNullException("definition");

            using (var stream =new MemoryStream(1024 * 1024))
            {
                definition.Write(stream);

                var result = Assembly.Load(stream.GetBuffer());

                return result != null ? Maybe<Assembly>.With(result) : Maybe<Assembly>.None;
            }
        }
    }
}
