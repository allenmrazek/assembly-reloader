using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AssemblyReloaderUnitTests.Fixture
{
    [KSPAddon(KSPAddon.Startup.Instantly, false)]
    internal class TestAddon_Private : MonoBehaviour { }

    [KSPAddon(KSPAddon.Startup.Instantly, false)]
    public class TestAddon_Public : MonoBehaviour { }

    [KSPAddon(KSPAddon.Startup.Instantly, false)]
    public class TestAddon_InvalidResult
    {
    }

    [KSPAddon(KSPAddon.Startup.Instantly, false)]
    public class TestAddon_InvalidResultPartModule : PartModule
    {
    }

    [KSPAddon(KSPAddon.Startup.Instantly, false)]
    [RequireComponent(typeof(AudioSource))]
    [Serializable]
    public class TestAddon_MultipleAttributes : MonoBehaviour
    {
        
    }

    public class IrrelevantAddonContainerClass
    {
        [KSPAddon(KSPAddon.Startup.Instantly, false)]
        private class TestAddon_Internal : MonoBehaviour { }
    }

    public class MonoBehaviour_WithNoAddon : MonoBehaviour
    {
    }
}
