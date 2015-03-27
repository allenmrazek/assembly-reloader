using System;
using System.Linq;
using AssemblyReloader.Annotations;
using AssemblyReloader.Game;
using AssemblyReloader.Providers;
using ReeperCommon.Extensions;
using UnityEngine;

namespace AssemblyReloader.CompositeRoot
{
    public class KspPartActionWindowListener : MonoBehaviour
    {
        public static IPartActionWindowController WindowController;
        public static IComponentsInGameObjectHierarchyProvider<UIPartActionWindow> PartActionWindowQuery;

        static KspPartActionWindowListener()
        {
            WindowController = null;
            PartActionWindowQuery = null;
        }


        [UsedImplicitly]
        private void Start()
        {
            if (WindowController.IsNull())
                throw new ArgumentException("WindowController null");
            if (PartActionWindowQuery.IsNull())
                throw new ArgumentException("PartActionWindowQuery null");

            WindowController.Add(PartActionWindowQuery.Get(gameObject).SingleOrDefault());
        }


        [UsedImplicitly]
        private void OnDestroy()
        {
            WindowController.Remove(PartActionWindowQuery.Get(gameObject).SingleOrDefault());
        }
    }
}
