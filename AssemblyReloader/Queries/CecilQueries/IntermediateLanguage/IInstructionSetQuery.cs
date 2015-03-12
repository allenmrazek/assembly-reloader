using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace AssemblyReloader.Queries.CecilQueries.IntermediateLanguage
{
    public interface IInstructionSetQuery
    {
        IEnumerable<Instruction> Get(MethodDefinition methodDefinition);
    }
}
