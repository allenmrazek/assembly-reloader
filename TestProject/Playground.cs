using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Contracts.Agents.Mentalities;
using ReeperCommon.Extensions;
using ReeperCommon.Logging.Implementations;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TestProject
{
    [KSPAddon(KSPAddon.Startup.EditorAny, false)]
    public class CheckRootModules : MonoBehaviour
    {
        private void Awake()
        {
            //if (EditorLogic.RootPart != null)
            //EditorLogic.RootPart.FindModulesImplementing<PartModule>()
            //    .ToList()
            //    .ForEach(m => print("Module: " + m.GetType().FullName));
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Keypad8))
            {
                print("Dumping loaded parts now ...");

                var loadedParts = Object.FindObjectsOfType(typeof(Part)).Cast<Part>().ToList();

                print(loadedParts.Count + " FindObjectsOfType returned");

                // Bit tricky here: loadedParts is looking for loose parts so if the parts are actually attached to each
                // other via parenting instead of joints (such as when building a ship in the editor), we'll only find
                // the top-level ones! Better look for children too
                loadedParts
                    .SelectMany(p => p.gameObject.GetComponentsInChildren<Part>(true))
                    .Where(p => !ReferenceEquals(p.gameObject, p.partInfo.partPrefab.gameObject))
                    .ToList()
                    .ForEach(p => print("Loaded part: " + p.name + ", " + p.flightID));
            }
        }
    }
}
