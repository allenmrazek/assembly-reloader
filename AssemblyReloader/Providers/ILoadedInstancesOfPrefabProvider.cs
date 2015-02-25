using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AssemblyReloader.Game;

namespace AssemblyReloader.Providers
{
    public interface ILoadedInstancesOfPrefabProvider
    {
        IEnumerable<IPart> Get(IPart prefab);
    }
}
