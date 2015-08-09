using System;
using Mono.Cecil;

namespace AssemblyReloader.ReloadablePlugin.Weaving.Operations
{
    public class NoConstructorArgumentFoundException : Exception
    {
        public NoConstructorArgumentFoundException() : base("No constructor arguments found")
        {
            
        }

        public NoConstructorArgumentFoundException(string message) : base(message)
        {
        }

        public NoConstructorArgumentFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
            
        }

        public NoConstructorArgumentFoundException(CustomAttribute attribute, TypeDefinition owner)
            : base("No constructor arguments found for " + attribute.AttributeType.FullName + " on " + owner.FullName)
        {
            
        }
    }
}
