using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssemblyReloader.Game
{
    class PartPrefabReferenceComparer : IEqualityComparer<IPart>
    {
        public bool Equals(IPart x, IPart y)
        {
            return ReferenceEquals(x.PartInfo.PartPrefab, y.PartInfo.PartPrefab);
        }

        public int GetHashCode(IPart obj)
        {
            return base.GetHashCode();
        }
    }
}
