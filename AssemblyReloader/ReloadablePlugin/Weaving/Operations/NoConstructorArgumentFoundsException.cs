extern alias Cecil96;
using System;
using CustomAttribute = Cecil96::Mono.Cecil.CustomAttribute;
using TypeDefinition = Cecil96::Mono.Cecil.TypeDefinition;

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
