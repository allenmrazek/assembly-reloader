using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AssemblyReloader.ReloadablePlugin.Weaving.Operations.GameEventInterception
{
    public class DuplicateGameEventRegistryException : Exception
    {
        public DuplicateGameEventRegistryException() : base("Duplicate game event registry")
        {
            
        }

        public DuplicateGameEventRegistryException(string message) : base(message)
        {
            
        }

        public DuplicateGameEventRegistryException(string message, Exception inner) : base(message, inner)
        {
            
        }
    }
}
