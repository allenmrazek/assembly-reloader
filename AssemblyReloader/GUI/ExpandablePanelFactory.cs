using System;
using ReeperCommon.Gui.Controls;
using UnityEngine;

namespace AssemblyReloader.Gui
{
    public class ExpandablePanelFactory : IExpandablePanelFactory
    {
        private readonly GUIStyle _toggleStyle;
        private readonly float _leftMargin;
        private readonly float _rightMargin;
        private readonly bool _initialState;

        public ExpandablePanelFactory(GUIStyle toggleStyle, float leftMargin, float rightMargin, bool initialState)
        {
            if (toggleStyle == null) throw new ArgumentNullException("toggleStyle");

            _toggleStyle = toggleStyle;
            _leftMargin = leftMargin;
            _rightMargin = rightMargin;
            _initialState = initialState;
        }


        public ICustomControl Create(string text, Action drawCall)
        {
            if (text == null) throw new ArgumentNullException("text");
            if (drawCall == null) throw new ArgumentNullException("drawCall");

            return new HorizontalMarginDecorator(_leftMargin, _rightMargin, new ExpandablePanel(_toggleStyle, text, drawCall, _initialState));
        }
    }
}
