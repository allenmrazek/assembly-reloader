using System;

namespace AssemblyReloader.Game
{
    public class DuplicatePartPrefabException : Exception
    {
        public DuplicatePartPrefabException(AvailablePart ap) : base("Duplicate part prefab: " + ap.name)
        {
            
        }
    }
}
