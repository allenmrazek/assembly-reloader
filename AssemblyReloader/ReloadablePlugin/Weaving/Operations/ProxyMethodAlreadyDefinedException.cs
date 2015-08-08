using System;
using Mono.Cecil;

namespace AssemblyReloader.ReloadablePlugin.Weaving.Operations
{
    public class ProxyMethodAlreadyDefinedException : Exception
    {
        public ProxyMethodAlreadyDefinedException(TypeDefinition typeDefinition, MethodDefinition method) :
            base(
            "TypeDefinition " + typeDefinition.FullName + " already has a method named \"" + method.FullName + "\" defined!")
        {
            
        }
    }
}
