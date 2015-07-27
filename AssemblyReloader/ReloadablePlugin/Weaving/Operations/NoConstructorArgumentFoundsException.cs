using System;
using Mono.Cecil;

namespace AssemblyReloader.ReloadablePlugin.Weaving.Operations
{
    class NoConstructorArgumentFoundsException : Exception
    {
        public NoConstructorArgumentFoundsException(CustomAttribute attribute, TypeDefinition owner)
            : base("No constructor arguments found for " + attribute.AttributeType.FullName + " on " + owner.FullName)
        {
            
        }
    }
}
