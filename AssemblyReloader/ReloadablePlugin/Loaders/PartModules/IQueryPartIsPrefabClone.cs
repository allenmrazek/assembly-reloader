using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssemblyReloader.ReloadablePlugin.Loaders.PartModules
{
    public interface IQueryPartIsPrefabClone
    {
        bool Get(IPart queryPart, IPart prefabPart);
    }
}
