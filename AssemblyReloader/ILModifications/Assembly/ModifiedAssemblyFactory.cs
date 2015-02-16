using System;
using Mono.Cecil;

namespace AssemblyReloader.ILModifications.Assembly
{
    public class ModifiedAssemblyFactory : IModifiedAssemblyFactory
    {
        public IModifiedAssembly Create(AssemblyDefinition definition)
        {
            if (definition == null) throw new ArgumentNullException("definition");

            return new ModifiedAssembly(definition);
        }
    }
}
