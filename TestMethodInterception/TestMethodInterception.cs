using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace TestMethodInterception
{
    public class TestMethodInterception : MonoBehaviour
    {
        private void Awake()
        {
            print("CodeBase (executing assembly): " + Assembly.GetExecutingAssembly().CodeBase);
            print("CodeBase (typeof assembly): " + typeof (TestMethodInterception).Assembly.CodeBase);

            print("Location (executing assembly): " + Assembly.GetExecutingAssembly().Location);
            print("Location (typeof assembly): " + typeof (TestMethodInterception).Assembly.Location);
        }
    }
}
