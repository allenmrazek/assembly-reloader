using System;
using System.Collections.Generic;
using AssemblyReloader.Annotations;
using UnityEngine;

namespace AssemblyReloader.Gui
{
    public class ExpandablePanelFactory : IExpandablePanelFactory
    {
        private readonly GUIStyle _toggleStyle;
        private readonly GUILayoutOption[] _toggleLayoutOptions;

        public ExpandablePanelFactory(
            [NotNull] GUIStyle toggleStyle,
            [NotNull] params GUILayoutOption[] toggleLayoutOptions)
        {
            if (toggleStyle == null) throw new ArgumentNullException("toggleStyle");
            if (toggleLayoutOptions == null) throw new ArgumentNullException("toggleLayoutOptions");

            _toggleStyle = toggleStyle;
            _toggleLayoutOptions = toggleLayoutOptions;
        }


        public IExpandablePanel Create(string label, float contentOffset, Action<IEnumerable<GUILayoutOption>> drawAction, bool startExpanded = false)
        {
            return new ExpandablePanel(_toggleStyle, label, contentOffset, drawAction, startExpanded, _toggleLayoutOptions);
        }
    }
}
