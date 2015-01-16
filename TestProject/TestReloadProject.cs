using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace TestProject
{
    [KSPAddon(KSPAddon.Startup.Instantly, false)]
    public class TestReloadProject : MonoBehaviour
    {
        void Start()
        {
            //gameObject.GetComponents<Component>().ToList().ForEach(c => print("Have component: " + c.name));
            WriteString("TestReloadProject: " + typeof(TestReloadProject).FullName);
        }

        void Update()
        {
            WriteString("TestReloadProject: " + typeof(TestReloadProject).FullName + " from " + Assembly.GetExecutingAssembly().Location);
            //var r = new SecondClass("secondclass create"); // to verify namespace change hasn't broken something internally
        }

        public static void WriteString(string str)
        {
            //print(str);
        }

        private void OnAssemblyReloadRequested()
        {
            print("TestReloadProject.OnAssemblyReloadRequested received!");
        }
    }
}
