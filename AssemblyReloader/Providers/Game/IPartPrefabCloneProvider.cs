using System.Collections.Generic;
using AssemblyReloader.Game;

namespace AssemblyReloader.Providers.Game
{
    public interface IPartPrefabCloneProvider
    {
        IEnumerable<IPart> Get(IPart prefab);
    }
}
