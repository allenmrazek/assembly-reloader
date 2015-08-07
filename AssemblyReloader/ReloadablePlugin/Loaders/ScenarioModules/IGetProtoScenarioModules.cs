using System;
using System.Collections.Generic;
using AssemblyReloader.Game;

namespace AssemblyReloader.ReloadablePlugin.Loaders.ScenarioModules
{
    public interface IGetProtoScenarioModules
    {
        IEnumerable<IProtoScenarioModule> Get(Type type);
    }
}
