using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Contracts.Agents.Mentalities;
using UnityEngine;

namespace TestProject.TestData
{
    [KSPAddon(KSPAddon.Startup.MainMenu, true)]
    public class KSPAddonMonoBehaviour : MonoBehaviour
    {
    }

    [KSPAddon(KSPAddon.Startup.Instantly, true)]
    public class KSPAddonDerived : LoadingSystem
    {
        
    }
}
