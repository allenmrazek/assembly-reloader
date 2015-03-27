﻿using System;
using Mono.Cecil;

namespace AssemblyReloader.Generators
{
    public class UniqueAssemblyNameGenerator : IUniqueAssemblyNameGenerator
    {
        public string Get(AssemblyDefinition definition)
        {
            return Guid.NewGuid() + "." + definition.Name;
        }
    }
}
