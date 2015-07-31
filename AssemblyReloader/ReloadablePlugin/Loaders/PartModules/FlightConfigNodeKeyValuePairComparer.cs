using System.Collections.Generic;
using AssemblyReloader.DataObjects;

namespace AssemblyReloader.ReloadablePlugin.Loaders.PartModules
{
    public class FlightConfigNodeKeyValuePairComparer : IEqualityComparer<KeyValuePair<uint, ITypeIdentifier>> 
    {
        public bool Equals(KeyValuePair<uint, ITypeIdentifier> x, KeyValuePair<uint, ITypeIdentifier> y)
        {
            return x.Key == y.Key && string.Equals(x.Value.Identifier, y.Value.Identifier);
        }

        public int GetHashCode(KeyValuePair<uint, ITypeIdentifier> obj)
        {
            return obj.Key.GetHashCode();
        }
    }
}
