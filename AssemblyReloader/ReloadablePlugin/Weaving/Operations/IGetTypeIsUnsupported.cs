using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;

namespace AssemblyReloader.ReloadablePlugin.Weaving.Operations
{
    public interface IGetTypeIsUnsupported
    {
        bool Get(TypeDefinition typeDefinition);
    }
}
