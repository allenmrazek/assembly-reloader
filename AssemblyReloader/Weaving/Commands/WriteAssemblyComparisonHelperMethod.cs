using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AssemblyReloader.CompositeRoot.Commands;
using Mono.Cecil;

namespace AssemblyReloader.Weaving.Commands
{
    public class WriteAssemblyComparisonHelperMethod : ICommand<TypeDefinition>
    {
        private readonly string _name;
        private readonly string _retValue;

        public WriteAssemblyComparisonHelperMethod(string name, string retValue)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentException("name cannot be null");
            if (string.IsNullOrEmpty(retValue)) throw new ArgumentException("retValue cannot be null");

            _name = name;
            _retValue = retValue;
        }


        public void Execute(TypeDefinition context)
        {
            var methodDefinition = new MethodDefinition(_name,
                MethodAttributes.HideBySig | MethodAttributes.Public | MethodAttributes.Static,
                context.Module.TypeSystem.String);

            context.Methods.Add(methodDefinition);

            //throw new NotImplementedException(); // todo: write actual il code here
        }
    }
}
