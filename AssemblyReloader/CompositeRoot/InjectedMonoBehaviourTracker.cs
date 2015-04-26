using AssemblyReloader.Annotations;
using ReeperCommon.Extensions;
using ReeperCommon.Logging;
using UnityEngine;

namespace AssemblyReloader.CompositeRoot
{
    /// <summary>
    /// This little class will help us keep an eye on every MonoBehaviour type reloadable plugins
    /// create
    /// </summary>
    public class InjectedMonoBehaviourTracker : MonoBehaviour
    {
        [UsedImplicitly]
        private void Start()
        {
            print("ART: Injected tracker running");
            gameObject.PrintComponents(new DebugLog("InjectedTracker"));
        }
    }
}
