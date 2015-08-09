using System;
using Mono.Cecil;

namespace AssemblyReloader.ReloadablePlugin.Weaving.Operations
{
    class NoConstructorArgumentFoundsException : Exception
    {
        public NoConstructorArgumentFoundsException() : base("No constructor arguments found")
        {
            
        }

        public NoConstructorArgumentFoundsException(string message) : base(message)
        {
        }

        public NoConstructorArgumentFoundsException(string message, Exception innerException)
            : base(message, innerException)
        {
            
        }

        public NoConstructorArgumentFoundsException(CustomAttribute attribute, TypeDefinition owner)
            : base("No constructor arguments found for " + attribute.AttributeType.FullName + " on " + owner.FullName)
        {
            
        }
    }
}
