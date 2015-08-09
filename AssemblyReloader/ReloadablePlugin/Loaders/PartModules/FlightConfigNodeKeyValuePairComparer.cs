using System.Collections.Generic;
using AssemblyReloader.DataObjects;

namespace AssemblyReloader.ReloadablePlugin.Loaders.PartModules
{
    public class FlightConfigNodeKeyValuePairComparer : IEqualityComparer<KeyValuePair<uint, TypeIdentifier>> 
    {
        public bool Equals(KeyValuePair<uint, TypeIdentifier> x, KeyValuePair<uint, TypeIdentifier> y)
        {
            return x.Key == y.Key && string.Equals(x.Value.Identifier, y.Value.Identifier);
        }

        public int GetHashCode(KeyValuePair<uint, TypeIdentifier> obj)
        {
            return obj.Key.GetHashCode();
        }
    }
}
