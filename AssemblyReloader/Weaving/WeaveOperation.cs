using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;

namespace AssemblyReloader.Weaving
{
    public abstract class WeaveOperation : IWeaveOperation
    {
        public abstract void Run(AssemblyDefinition definition);

        public virtual void OnEachType(TypeDefinition typeDefinition)
        {
            
        }

        public virtual void OnEachMethod(MethodDefinition methodDefinition)
        {
            
        }
    }
}
