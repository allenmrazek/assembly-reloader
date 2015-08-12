using System;
using AssemblyReloader.ReloadablePlugin.Loaders;

namespace AssemblyReloader.Game
{
    public class PrefabNotFoundException : Exception
    {
        public PrefabNotFoundException() : base("Failed to find part prefab")
        {
            
        }


        public PrefabNotFoundException(IPart fromPart)
            : base("Failed to find part prefab for \"" + fromPart.PartInfo.Name + "\"")
        {
            
        }


        public PrefabNotFoundException(string message)
            :base(message)
        {
            
        }

        public PrefabNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
            
        }
    }
}
