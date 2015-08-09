using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AssemblyReloader.DataObjects;

namespace AssemblyReloader.ReloadablePlugin.Loaders.ScenarioModules
{
    public class TypeIdentifierComparer : IEqualityComparer<TypeIdentifier>
    {
        public bool Equals(TypeIdentifier x, TypeIdentifier y)
        {
            return x.Identifier == y.Identifier;
        }

        public int GetHashCode(TypeIdentifier obj)
        {
            return obj.GetHashCode();
        }
    }
}
