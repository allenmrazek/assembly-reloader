using System;
using Mono.Cecil;
using ReeperCommon.Containers;

namespace AssemblyReloader.ILModifications.Assembly
{
    public class ModifiedAssembly : IModifiedAssembly
    {
        private readonly AssemblyDefinition _assemblyDefinition;

        public ModifiedAssembly(AssemblyDefinition assemblyDefinition)
        {
            if (assemblyDefinition == null) throw new ArgumentNullException("assemblyDefinition");
            _assemblyDefinition = assemblyDefinition;
        }

        public void Rename(Guid newId)
        {
            _assemblyDefinition.Name.Name = newId.ToString() + "." + _assemblyDefinition.Name.Name;
        }

        public void Trampoline(MethodDefinition @from, MethodDefinition to)
        {
            throw new NotImplementedException();
        }

        public MethodDefinition CreateMethod(ModuleDefinition module, TypeDefinition type, string name, MethodAttributes attr)
        {
            if (type == null) throw new ArgumentNullException("type");

            var voidReference = module.Import(typeof(void));

            var method = new MethodDefinition(name, MethodAttributes.Private, voidReference);
            type.Methods.Add(method);

            return method;
        }
    }
}
