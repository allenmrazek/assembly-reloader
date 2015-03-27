namespace TestProject.TestData
{
    [KSPScenario(ScenarioCreationOptions.AddToAllGames, new []{ GameScenes.SPACECENTER })]
    public class ScenarioModuleTest : ScenarioModule
    {
        public override void OnAwake()
        {
            base.OnAwake();
            print("ScenarioModuleTest.OnAwake");
        }

        public override void OnLoad(ConfigNode node)
        {
            base.OnLoad(node);
            print("ScenarioModuleTest.OnLoad: " + node.ToString());
        }

        public override void OnSave(ConfigNode node)
        {
            base.OnSave(node);
            print("ScenarioModuleTest.OnSave");
            node.AddValue("TestName", "TestValue");
        }

        private void OnPluginReloadRequested()
        {
            print("ScenarioModuleTest.OnPluginReloadRequested received");
        }
    }
}
