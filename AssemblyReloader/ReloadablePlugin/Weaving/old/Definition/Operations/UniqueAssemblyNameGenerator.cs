using System;
using AssemblyReloader.Properties;
using AssemblyReloader.StrangeIoC.extensions.implicitBind;
using Mono.Cecil;

namespace AssemblyReloader.ReloadablePlugin.Weaving.old.Definition.Operations
{
    [Implements(typeof(IUniqueAssemblyNameGenerator))]
    public class UniqueAssemblyNameGenerator : IUniqueAssemblyNameGenerator
    {
        private readonly IGetRandomString _string;

        public UniqueAssemblyNameGenerator([NotNull] IGetRandomString @string)
        {
            if (@string == null) throw new ArgumentNullException("string");
            _string = @string;
        }

        public string Get(AssemblyDefinition definition)
        {
            return _string.Get() + "." + definition.Name;
        }
    }
}
