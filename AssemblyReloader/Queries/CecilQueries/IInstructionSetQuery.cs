using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace AssemblyReloader.Queries.CecilQueries
{
    public interface IInstructionSetQuery
    {
        IEnumerable<Instruction> Get(MethodDefinition methodDefinition);
    }
}
