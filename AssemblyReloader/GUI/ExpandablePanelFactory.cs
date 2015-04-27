using System;
using System.Collections.Generic;
using AssemblyReloader.Annotations;
using UnityEngine;

namespace AssemblyReloader.Gui
{
    public class ExpandablePanelFactory : IExpandablePanelFactory
    {
        private readonly GUISkin _panelSkin;
        private readonly GUILayoutOption[] _toggleLayoutOptions;

        public ExpandablePanelFactory([NotNull] GUISkin panelSkin,
            [NotNull] params GUILayoutOption[] toggleLayoutOptions)
        {
            if (panelSkin == null) throw new ArgumentNullException("panelSkin");
            if (toggleLayoutOptions == null) throw new ArgumentNullException("toggleLayoutOptions");

            _panelSkin = panelSkin;
            _toggleLayoutOptions = toggleLayoutOptions;
        }


        public IExpandablePanel Create(string label, float contentOffset, Action<IEnumerable<GUILayoutOption>> drawAction, bool startExpanded = false)
        {
            return new ExpandablePanel(_panelSkin, label, contentOffset, drawAction, startExpanded, _toggleLayoutOptions);
        }
    }
}
