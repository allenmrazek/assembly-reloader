using System.Collections.Generic;
using AssemblyReloader.Game;

namespace AssemblyReloader.ReloadablePlugin.Loaders.PartModules
{
    public interface IGetPartPrefabClones
    {
        IEnumerable<IPart> Get(IPart prefab);
    }
}
