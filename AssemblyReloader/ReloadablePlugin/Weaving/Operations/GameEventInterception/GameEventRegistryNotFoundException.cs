using System;

namespace AssemblyReloader.ReloadablePlugin.Weaving.Operations.GameEventInterception
{
    public class GameEventRegistryNotFoundException : Exception
    {
        public GameEventRegistryNotFoundException() : base("GameEvent registry was not found")
        {
            
        }

        public GameEventRegistryNotFoundException(string message) : base(message)
        {
            
        }

        public GameEventRegistryNotFoundException(string message, Exception inner) : base(message, inner)
        {
            
        }
    }
}
