using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;

namespace AssemblyReloader.Weaving
{
    public interface IWeaveOperation
    {
        void Run(AssemblyDefinition definition);
        void OnEachType(TypeDefinition typeDefinition);
        void OnEachMethod(MethodDefinition methodDefinition);
    }
}
