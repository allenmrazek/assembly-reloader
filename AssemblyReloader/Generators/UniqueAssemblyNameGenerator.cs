using System;
using AssemblyReloader.Annotations;
using Mono.Cecil;

namespace AssemblyReloader.Generators
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
