using System;
using System.Collections.Generic;
using AssemblyReloader.DataObjects;
using UnityEngine;

namespace AssemblyReloader.Gui
{
    public interface IExpandablePanelFactory
    {
        IExpandablePanel Create(
            string label,
            float contentOffset,
            Action<IEnumerable<GUILayoutOption>> drawAction,
            bool startExpanded);
    }
}
