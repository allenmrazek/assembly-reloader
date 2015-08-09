using System;

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
    }
}
