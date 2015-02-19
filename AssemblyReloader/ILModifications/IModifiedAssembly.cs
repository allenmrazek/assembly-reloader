using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Mono.Cecil;
using ReeperCommon.Containers;
using MethodAttributes = Mono.Cecil.MethodAttributes;

namespace AssemblyReloader.ILModifications
{
    public interface IModifiedAssembly
    {
        void Rename(Guid guid);
        void Trampoline(MethodDefinition from, MethodDefinition to);

        MethodDefinition CreateMethod(ModuleDefinition module, TypeDefinition type, string name, MethodAttributes attr);
        IEnumerable<MethodDefinition> FindMethod(TypeDefinition type, string name);

        void Write(MemoryStream stream);
        Maybe<Assembly> Load(MemoryStream source);
    }
}
