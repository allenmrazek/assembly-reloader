using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssemblyReloader.Game
{
    public interface IGame
    {
        bool RemoveProtoScenarioModule(Type scnType);
        void AddProtoScenarioModule(ConfigNode scnConfig);
        void AddProtoScenarioModule(Type scnType, params GameScenes[] targetScenes);
    }
}
