using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace TestProject.TestData
{
    class ClassThatAccessesKSPAddonAttribute : MonoBehaviour
    {
        public void ListKSPAddonsInThisAssembly()
        {
            var addons = GetType().Assembly.GetTypes().Where(t => t.GetCustomAttributes(typeof (KSPAddon), true).Length > 0);

            foreach (var addon in addons)
                print("Addon found: " + addon.FullName);
        }
    }
}
