using System.Collections.Generic;

namespace AssemblyReloader.Game.Providers
{
    public interface IGetPartPrefabClones
    {
        IEnumerable<IPart> Get(IPart prefab);
    }
}
