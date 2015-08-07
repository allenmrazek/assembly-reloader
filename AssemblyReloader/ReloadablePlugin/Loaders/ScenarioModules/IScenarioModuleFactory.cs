using System;
using AssemblyReloader.Game;
using ReeperCommon.Containers;

namespace AssemblyReloader.ReloadablePlugin.Loaders.ScenarioModules
{
    public interface IScenarioModuleFactory
    {
        Maybe<ScenarioModule> Create(IProtoScenarioModule psm, Type type);
    }
}
