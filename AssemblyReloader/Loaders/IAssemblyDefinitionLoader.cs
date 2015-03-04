﻿using System.Reflection;
using Mono.Cecil;
using ReeperCommon.Containers;

namespace AssemblyReloader.Loaders
{
    public interface IAssemblyDefinitionLoader
    {
        Maybe<Assembly> Load(AssemblyDefinition definition);
    }
}
