using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;

namespace AssemblyReloader.Providers
{
    public interface IUniqueAssemblyNameProvider
    {
        string Get(AssemblyDefinition definition);
    }
}
