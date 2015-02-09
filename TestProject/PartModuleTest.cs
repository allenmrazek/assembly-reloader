using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestProject
{
// ReSharper disable once InconsistentNaming
    public class TestPartModule : PartModule
    {
        public override void OnAwake()
        {
 	        base.OnAwake();

#if MODIFIED
            print("TestPartModule awake (**MODIFIED** version)");
#else
            print("TestPartModule awake (unmodified version)");
#endif
        }

    }
}
