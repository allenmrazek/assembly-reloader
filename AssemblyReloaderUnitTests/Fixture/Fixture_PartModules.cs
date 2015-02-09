using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssemblyReloaderUnitTests.Fixture
{
    class TestPartModule : PartModule
    {

    }

    class DerivedPartModule : TestPartModule
    {

    }

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
