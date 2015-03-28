using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReeperCommon.Containers;

namespace AssemblyReloader.Game
{
    public interface IProtoScenarioModule
    {
        Maybe<ScenarioModule> moduleRef { get; set; }
        string moduleName { get; }
        GameScenes[] TargetScenes { get; set; }

        Maybe<ScenarioModule> Load();
    }
}
