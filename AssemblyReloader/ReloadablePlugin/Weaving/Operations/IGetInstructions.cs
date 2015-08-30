extern alias Cecil96;
using System.Collections.Generic;
using Instruction = Cecil96::Mono.Cecil.Cil.Instruction;
using MethodDefinition = Cecil96::Mono.Cecil.MethodDefinition;

namespace AssemblyReloader.ReloadablePlugin.Weaving.Operations
{
    public interface IGetInstructionsInMethod
    {
        IEnumerable<Instruction> Get(MethodDefinition methodDefinition);
    }
}
