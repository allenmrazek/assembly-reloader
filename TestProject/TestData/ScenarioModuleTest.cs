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
