using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;

namespace AssemblyReloader.Providers
{
    public class UniqueAssemblyNameProvider : IUniqueAssemblyNameProvider
    {
        public string Get(AssemblyDefinition definition)
        {
            return Guid.NewGuid() + "." + definition.Name;
        }
    }
}
