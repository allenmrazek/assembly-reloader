using System.Collections.Generic;

namespace AssemblyReloader.Game
{
    public interface IScenarioRunner
    {
        List<IProtoScenarioModule> GetUpdatedProtoScenarioModules();
    }
}
