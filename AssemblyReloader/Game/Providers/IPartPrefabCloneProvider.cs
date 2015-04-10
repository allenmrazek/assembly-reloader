using System.Collections.Generic;

namespace AssemblyReloader.Game.Providers
{
    public interface IPartPrefabCloneProvider
    {
        IEnumerable<IPart> Get(IPart prefab);
    }
}
