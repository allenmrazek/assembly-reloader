using System;
using Mono.Cecil;
using ReeperCommon.Containers;

namespace AssemblyReloader.ILModifications.Assembly
{
    public interface IModifiedAssembly
    {
        void Rename(Guid guid);
        void Trampoline(MethodDefinition from, MethodDefinition to);

        MethodDefinition CreateMethod(ModuleDefinition module, TypeDefinition type, string name, MethodAttributes attr);
    }
}
