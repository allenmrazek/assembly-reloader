using UnityEngine;

namespace AssemblyReloader.Gui
{
    public interface IExpandablePanel
    {
        void Draw(params GUILayoutOption[] options);
        bool Expanded { get; set; }
    }
}
