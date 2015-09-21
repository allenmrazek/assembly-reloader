using UnityEngine;

namespace TestAddon
{
    [KSPAddon(KSPAddon.Startup.Instantly, false)]
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
}
