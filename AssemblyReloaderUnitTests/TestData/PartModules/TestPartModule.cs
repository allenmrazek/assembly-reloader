namespace AssemblyReloaderTests.TestData.PartModules
{
    class TestPartModule : PartModule
    {
        public override void OnLoad(ConfigNode node)
        {
            base.OnLoad(node);
        }

        public override void OnSave(ConfigNode node)
        {
            base.OnSave(node);
        }
    }
}