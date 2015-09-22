using UnityEngine;

namespace TestAddon
{
    [KSPAddon(KSPAddon.Startup.Instantly, true)]
    public class TestAddon : MonoBehaviour
    {
        private void Start()
        {
#if MODIFIED
            print("TestAddon.Start - *modified*");
#else
            print("TestAddon.Start - unmodified");
#endif
        }
    }

    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public class TestAddonInFlight : MonoBehaviour
    {
        private void Start()
        {
            print("TestAddonInFlight.Start");
        }
    }
}
