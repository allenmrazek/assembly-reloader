using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestProject
{
// ReSharper disable once InconsistentNaming
    public class TestPartModule : PartModule
    {
        ////void Awake()
        ////{
        ////    print("TestPartModule awake");
        ////}

        public override void OnAwake()
        {
            base.OnAwake();

#if MODIFIED
            print("TestPartModule awake (**MODIFIED** version)");
#else
            print("TestPartModule awake (unmodified version)");
#endif
        }

        public override void OnLoad(ConfigNode node)
        {
            print(string.Format("TestPartModule.OnLoad: {0}", node.ToString()));
        }
    }


    public class DerivativePartModule : TestPartModule
    {
        
    }

    internal class InternalPartModule : PartModule
    {
    }
}
