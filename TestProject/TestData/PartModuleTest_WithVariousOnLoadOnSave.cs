namespace TestProject.TestData
{
    class PartModuleTest_WithVariousOnLoadOnSave : PartModule
    {
        // correct version
        public override void OnLoad(ConfigNode node)
        {
            
        }

        public override void OnSave(ConfigNode node)
        {
            
        }

        // bad version, no params
        public void OnLoad()
        {
            
        }

        public void OnSave()
        {
            
        }

        // bad version, wrong parameter type
        private void OnLoad(string @any)
        {
            
        }

        private void OnSave(string @any)
        {
            
        }

        // bad version, wrong number of parameters but includes a correct one
        private void OnLoad(ConfigNode node, string @any)
        {
            
        }

        private void OnSave(ConfigNode node, string @any)
        {
            
        }
    }
}
