extern alias KSP;
using System;

namespace AssemblyReloader.Game
{
    public class DuplicatePartPrefabException : Exception
    {
        public DuplicatePartPrefabException() : base("Duplicate part prefab")
        {
            
        }

        public DuplicatePartPrefabException(KSP::AvailablePart ap)
            : base("Duplicate part prefab: " + ap.name)
        {
            
        }

        public DuplicatePartPrefabException(string message) : base(message)
        {
            
        }

        public DuplicatePartPrefabException(string message, Exception innerException)
            : base(message, innerException)
        {
            
        }
    }
}
