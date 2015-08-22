using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssemblyReloaderTests.InProgress
{
    public static class TestReplacement
    {
        public static void Register<T>(EventData<T> evt, EventData<T>.OnEvent del)
        {
            
        }

        public static void Register<T1, T2>(EventData<T1, T2> evt, EventData<T1, T2>.OnEvent del)
        {

        }

        public static void Register<T1, T2, T3>(EventData<T1, T2, T3> evt, EventData<T1, T2, T3>.OnEvent del)
        {

        }


        public static void Register(EventVoid evt, EventVoid.OnEvent del)
        {
            
        }
    }
}
