using System;
using System.Linq;
using AssemblyReloader.CompositeRoot.Commands;
using AssemblyReloader.Game;
using AssemblyReloader.Providers;
using ReeperCommon.Extensions;
using UnityEngine;

namespace AssemblyReloader.CompositeRoot.MonoBehaviours
{
    public class KspPartActionWindowListener : MonoBehaviour
    {
        public static IPartActionWindowController WindowController;
        public static IComponentsInGameObjectHierarchyProvider<UIPartActionWindow> PartActionWindowQuery;
 
        private void Start()
        {
            if (WindowController.IsNull())
                throw new ArgumentException("WindowController null");
            if (PartActionWindowQuery.IsNull())
                throw new ArgumentException("PartActionWindowQuery null");

            WindowController.Add(PartActionWindowQuery.Get(gameObject).SingleOrDefault());
        }

        private void OnDestroy()
        {
            WindowController.Remove(PartActionWindowQuery.Get(gameObject).SingleOrDefault());
        }
    }
}
