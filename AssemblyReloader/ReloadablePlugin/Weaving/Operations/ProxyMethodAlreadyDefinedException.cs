using System;
using Mono.Cecil;

namespace AssemblyReloader.ReloadablePlugin.Weaving.Operations
{
    public class ProxyMethodAlreadyDefinedException : Exception
    {
        public ProxyMethodAlreadyDefinedException() : base("Proxy method has already been defined")
        {
            
        }

        public ProxyMethodAlreadyDefinedException(string message) : base(message)
        {
            
        }

        public ProxyMethodAlreadyDefinedException(string message, Exception innerException)
            : base(message, innerException)
        {
            
        }


        public ProxyMethodAlreadyDefinedException(TypeDefinition typeDefinition, MethodDefinition method) :
            base(
            "TypeDefinition " + typeDefinition.FullName + " already has a method named \"" + method.FullName + "\" defined!")
        {
            
        }
    }
}
