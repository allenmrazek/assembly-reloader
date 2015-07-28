using System.Runtime.InteropServices;
using UnityEngine;

namespace TestProject.TestData
{
    [KSPAddon(KSPAddon.Startup.MainMenu, false)]
    public class KSPAddonMonoBehaviour : MonoBehaviour
    {
    }

    [KSPAddon(KSPAddon.Startup.Instantly, true)]
    public class KSPAddonDerived : LoadingSystem
    {
        
    }
}
