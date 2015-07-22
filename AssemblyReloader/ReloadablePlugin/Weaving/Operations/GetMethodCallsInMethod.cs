using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;
using ReeperCommon.Extensions;

namespace AssemblyReloader.ReloadablePlugin.Weaving.Operations
{
    public class GetMethodCallsInMethod : IGetInstructionsInMethod
    {
        private readonly MethodInfo _methodInfo;
        private readonly OpCode _code;

        public GetMethodCallsInMethod(MethodInfo methodInfo, OpCode code)
        {
            if (methodInfo == null) throw new ArgumentNullException("methodInfo");
            if (code != OpCodes.Call && code != OpCodes.Callvirt && code != OpCodes.Calli)
                throw new ArgumentException("code must be Call, Callvirt or Calli");

            _methodInfo = methodInfo;
            _code = code;
        }


        public IEnumerable<Instruction> Get(MethodDefinition methodDefinition)
        {
            if (methodDefinition == null) throw new ArgumentNullException("methodDefinition");
            if (methodDefinition.Body == null) return new Instruction[] {};

            var target = methodDefinition.Module.Import(_methodInfo);

            return (from inst in methodDefinition.Body.Instructions 
                    where inst.OpCode == _code 
                    let possibleMethod = inst.Operand as MethodReference 
                    where !possibleMethod.IsNull() 
                    where target.FullName == possibleMethod.FullName 
                    select inst);
        }
    }
}
