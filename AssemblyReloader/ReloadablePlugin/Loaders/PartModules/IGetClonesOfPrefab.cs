using System.Collections.Generic;

namespace AssemblyReloader.ReloadablePlugin.Loaders.PartModules
{
    public interface IGetClonesOfPrefab
    {
        IEnumerable<IPart> Get(IPart prefab);
    }
}
