using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace AssemblyReloader.ReloadablePlugin.Weaving.Operations
{
    public interface IGetInstructionsInMethod
    {
        IEnumerable<Instruction> Get(MethodDefinition methodDefinition);
    }
}
