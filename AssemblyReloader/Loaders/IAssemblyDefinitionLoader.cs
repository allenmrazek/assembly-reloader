using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Mono.Cecil;
using ReeperCommon.Containers;

namespace AssemblyReloader.Loaders
{
    public interface IAssemblyDefinitionLoader
    {
        Maybe<Assembly> Load(AssemblyDefinition definition);
    }
}
