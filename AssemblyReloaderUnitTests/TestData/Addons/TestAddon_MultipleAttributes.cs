using System;
using UnityEngine;

namespace AssemblyReloaderTests.TestData.Addons
{
    [KSPAddon(KSPAddon.Startup.Instantly, false)]
    [RequireComponent(typeof(AudioSource))]
    [Serializable]
    public class TestAddon_MultipleAttributes : MonoBehaviour
    {
        
    }
}