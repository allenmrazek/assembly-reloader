using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestPartModule
{
    public class TestPartModule : PartModule
    {
        public override void OnStart(PartModule.StartState state)
        {
            base.OnStart(state);
#if MODIFIED
            print("TestPartModule.OnStart - *modified*");
#else
            print("TestPartModule.OnStart - unmodified");
#endif
        }
    }
}
