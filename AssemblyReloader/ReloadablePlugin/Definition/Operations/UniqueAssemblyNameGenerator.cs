using System;
using AssemblyReloader.FileSystem;
using AssemblyReloader.Properties;
using AssemblyReloader.StrangeIoC.extensions.implicitBind;
using Mono.Cecil;

namespace AssemblyReloader.ReloadablePlugin.Definition.Operations
{
    [Implements(typeof(IUniqueAssemblyNameGenerator))]
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
