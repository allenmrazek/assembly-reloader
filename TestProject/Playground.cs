using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Contracts.Agents.Mentalities;
using ReeperCommon.Extensions;
using ReeperCommon.Logging.Implementations;
using UnityEngine;

namespace TestProject
{
    [KSPAddon(KSPAddon.Startup.EditorAny, false)]
    public class CheckRootModules : MonoBehaviour
    {
        private void Awake()
        {
            EditorLogic.RootPart.FindModulesImplementing<PartModule>()
                .ToList()
                .ForEach(m => print("Module: " + m.GetType().FullName));
        }
    }
}
