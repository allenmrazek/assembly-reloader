using System;
using AssemblyReloader.FileSystem;
using AssemblyReloader.Properties;
using Mono.Cecil;

namespace AssemblyReloader.ReloadablePlugin.Definition.Operations
{
    public class UniqueAssemblyNameGenerator : IUniqueAssemblyNameGenerator
    {
        private readonly IRandomStringGenerator _stringGenerator;

        public UniqueAssemblyNameGenerator([NotNull] IRandomStringGenerator stringGenerator)
        {
            if (stringGenerator == null) throw new ArgumentNullException("stringGenerator");
            _stringGenerator = stringGenerator;
        }

        public string Get(AssemblyDefinition definition)
        {
            return _stringGenerator.Get() + "." + definition.Name;
        }
    }
}
