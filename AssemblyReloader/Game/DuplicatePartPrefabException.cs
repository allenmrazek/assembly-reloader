using System;

namespace AssemblyReloader.Game
{
    public class DuplicatePartPrefabException : Exception
    {
        public DuplicatePartPrefabException() : base("Duplicate part prefab")
        {
            
        }

        public DuplicatePartPrefabException(AvailablePart ap) : base("Duplicate part prefab: " + ap.name)
        {
            
        }
    }
}
