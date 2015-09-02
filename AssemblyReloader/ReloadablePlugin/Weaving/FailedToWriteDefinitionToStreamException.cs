extern alias Cecil96;
using System;

namespace AssemblyReloader.ReloadablePlugin.Weaving
{
    public class FailedToWriteDefinitionToStreamException : Exception
    {
        public FailedToWriteDefinitionToStreamException() : base("Failed to write assembly definition to stream")
        {
            
        }

        public FailedToWriteDefinitionToStreamException(string message) : base(message)
        {
            
        }

        public FailedToWriteDefinitionToStreamException(string message, Exception inner) : base(message, inner)
        {
            
        }

        public FailedToWriteDefinitionToStreamException(Cecil96::Mono.Cecil.AssemblyDefinition definition)
            : base("Failed to write definition of " + definition.Name + " to stream")
        {
            
        }
    }
}
