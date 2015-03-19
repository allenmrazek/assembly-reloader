namespace AssemblyReloaderTests.TestData.PartModules
{
    public class PartModuleContainerClass
    {
        
        public class InnerPartModule : PartModule
        {

        }

        public class DerivedInnerPartModule : InnerPartModule
        {

        }

        private class HiddenInternalPartModule : PartModule
        {
        }
    }
}