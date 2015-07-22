using System.Reflection;
using UnityEngine;

namespace TestProject.TestData.Intercepted
{
    public class TestCodeBaseInterception : MonoBehaviour
    {
        private void Awake()
        {
            print("This object instantiated from CodeBase " + Assembly.GetExecutingAssembly().CodeBase);
        }
    }
}
