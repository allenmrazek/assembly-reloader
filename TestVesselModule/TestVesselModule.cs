using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestVesselModule
{
    public class TestVesselModule : VesselModule
    {
        private void Start()
        {
#if MODIFIED
            print("TestVesselModule.Start - *modified*");
#else
            print("TestVesselModule.Start - unmodified");
#endif
        }
    }
}
