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
            print("TestVesselModule.Start - *modified* immediator");
#else
            print("TestVesselModule.Start - unmodified");
#endif
        }


        protected override void OnSave(ConfigNode node)
        {
            base.OnSave(node);
            node.AddValue("created", "inside TestVesselModule.OnSave");
        }

        protected override void OnLoad(ConfigNode node)
        {
            base.OnLoad(node);
            print("TestVesselModule.OnLoad: " + node.ToString());
        }
    }
}
