using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestProject.TestData
{
    [KSPScenario(ScenarioCreationOptions.AddToAllGames, new []{ GameScenes.SPACECENTER })]
    public class ScenarioModuleTest : ScenarioModule
    {
        private void Start()
        {
            print("ScenarioModuleTest: running from TestProject");
        }
    }
}
