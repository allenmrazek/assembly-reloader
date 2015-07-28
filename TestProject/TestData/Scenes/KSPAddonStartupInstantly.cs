using UnityEngine;

namespace TestProject.TestData.Scenes
{
    [KSPAddon(KSPAddon.Startup.Instantly, false)]
    class KSPAddonStartupInstantly : MonoBehaviour
    {
        private void Awake()
        {
            print(GetType().FullName + " awake");
        }
    }
}
