namespace TestProject.TestData
{
    class PartModuleTest_WithVariousOnLoad : PartModule
    {
        // correct version
        public override void OnLoad(ConfigNode node)
        {
            
        }

        // bad version, no params
        public void OnLoad()
        {
            
        }

        // bad version, wrong parameter type
        private void OnLoad(string @any)
        {
            
        }

        // bad version, wrong number of parameters but includes a correct one
        private void OnLoad(ConfigNode node, string @any)
        {
            
        }
    }
}
