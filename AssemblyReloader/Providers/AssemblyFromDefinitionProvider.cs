using System;
using System.IO;
using System.Linq;
using System.Reflection;
using AssemblyReloader.Disk;
using Mono.Cecil;
using ReeperCommon.Containers;
using ReeperCommon.Logging.Implementations;

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

                if (result != null)
                {
                    var log = new DebugLog("AssemblyFromDefinitionProvider");

                    result.GetReferencedAssemblies().ToList().ForEach(an => log.Normal(an.FullName + ", " + an.Version));

                }
                return result != null ? Maybe<Assembly>.With(result) : Maybe<Assembly>.None;
            }
        }
    }
}
