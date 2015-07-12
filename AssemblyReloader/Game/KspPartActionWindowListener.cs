using System;
using System.Linq;
using AssemblyReloader.Properties;
using AssemblyReloader.Providers;
using ReeperCommon.Extensions;
using UnityEngine;

namespace AssemblyReloader.Game
{
    public class KspPartActionWindowListener : MonoBehaviour
    {
// ReSharper disable once FieldCanBeMadeReadOnly.Global
        public static IPartActionWindowController WindowController;
// ReSharper disable once FieldCanBeMadeReadOnly.Global
        public static IGetComponentsInGameObjectHierarchy<UIPartActionWindow> PartActionWindowQuery;

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
