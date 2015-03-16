using System;
using ReeperCommon.Extensions;
using ReeperCommon.Logging.Implementations;
using UnityEngine;

namespace AssemblyReloader.CompositeRoot.MonoBehaviours
{
    /// <summary>
    /// This little class will help us keep an eye on every MonoBehaviour type reloadable plugins
    /// create
    /// </summary>
    public class InjectedMonoBehaviourTracker : MonoBehaviour
    {
        private void Start()
        {
            print("ART: Injected tracker running");
            gameObject.PrintComponents(new DebugLog("InjectedTracker"));
        }
    }
}
