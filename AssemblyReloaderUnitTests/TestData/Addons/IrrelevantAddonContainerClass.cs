using UnityEngine;

namespace AssemblyReloaderUnitTests.TestData.Addons
{
    public class IrrelevantAddonContainerClass
    {
        [KSPAddon(KSPAddon.Startup.Instantly, false)]
        private class TestAddon_Internal : MonoBehaviour { }
    }
}