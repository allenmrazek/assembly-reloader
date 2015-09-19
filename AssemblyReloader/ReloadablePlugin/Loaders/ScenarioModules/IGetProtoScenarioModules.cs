using System;
using System.Collections.Generic;

namespace AssemblyReloader.ReloadablePlugin.Loaders.ScenarioModules
{
    public interface IGetProtoScenarioModules
    {
        IEnumerable<IProtoScenarioModule> Get(Type type);
    }
}
