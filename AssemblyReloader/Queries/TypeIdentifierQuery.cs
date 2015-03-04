﻿using System;
using AssemblyReloader.DataObjects;

namespace AssemblyReloader.Queries
{
    // Where type identifier is the name KSP would use to search for this type in assemblies loaded by
    // its AssemblyLoader
    public class TypeIdentifierQuery : ITypeIdentifierQuery
    {
        public ITypeIdentifier Get(Type type)
        {
            return new TypeIdentifier(type.Name);
        }
    }
}