namespace TestScenarioModule
{
    [KSPScenario(ScenarioCreationOptions.AddToAllGames, new []{ GameScenes.SPACECENTER, GameScenes.EDITOR, GameScenes.FLIGHT, GameScenes.TRACKSTATION})]
    public class TestScenarioModule : ScenarioModule
    {
        public override void OnAwake()
        {
            base.OnAwake();
#if MODIFIED
            print("TestScenarioModule.OnAwake - *modified*");
#else
            print("TestScenarioModule.OnAwake - unmodified");
#endif
        }

        public override void OnLoad(ConfigNode node)
        {
            base.OnLoad(node);
#if MODIFIED
            print("TestScenarioModule.OnLoad - *modified*");
#else
            print("TestScenarioModule.OnLoad - unmodified");
#endif
        }


        private void Start()
        {
            print("TestScenarioModule.Start");
        }
    }
}
