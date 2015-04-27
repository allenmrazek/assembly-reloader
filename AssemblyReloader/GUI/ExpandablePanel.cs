using System;
using System.Collections.Generic;
using AssemblyReloader.Annotations;
using UnityEngine;

namespace AssemblyReloader.Gui
{
    public class ExpandablePanel : IExpandablePanel
    {
        private readonly GUISkin _skin;
        private readonly string _label;
        private readonly float _contentOffset;
        private readonly Action<IEnumerable<GUILayoutOption>> _drawMethod;
        private readonly GUILayoutOption[] _toggleOptions;
        

        public ExpandablePanel(
            [NotNull] GUISkin skin,
            string label,
            float contentOffset, 
            [NotNull] Action<IEnumerable<GUILayoutOption>> drawMethod, 
            bool initialState = false,
            params GUILayoutOption[] toggleOptions)
        {
            if (skin == null) throw new ArgumentNullException("skin");
            if (drawMethod == null) throw new ArgumentNullException("drawMethod");

            _skin = skin;
            _label = label;
            _contentOffset = contentOffset;
            _drawMethod = drawMethod;
            _toggleOptions = toggleOptions;
            Expanded = initialState;

            //toggleStyle.normal.background.SaveToDisk("toggle_normal_background.png");
            //toggleStyle.active.background.SaveToDisk("toggle_active_background.png");
            //toggleStyle.onNormal.background.SaveToDisk("toggle_onnormal_background.png");
            //toggleStyle.onActive.background.SaveToDisk("toggle_onactive_background.png");
        }


        public void Draw(params GUILayoutOption[] options)
        {
            var prevSkin = GUI.skin;

            GUI.skin = _skin;

            Expanded = GUILayout.Toggle(Expanded, _label, _toggleOptions);

            GUI.skin = prevSkin;

            GUILayout.BeginHorizontal(options);
            if (Expanded)
            {
                GUILayout.Space(_contentOffset);
                GUILayout.BeginVertical(options);
                _drawMethod(options);
                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();
        }


        public bool Expanded { get; set; }
    }
}
