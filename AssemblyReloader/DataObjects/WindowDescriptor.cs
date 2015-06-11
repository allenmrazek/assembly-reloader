using System;
using AssemblyReloader.Annotations;
using ReeperCommon.Gui.Window;
using ReeperCommon.Gui.Window.View;

namespace AssemblyReloader.DataObjects
{
    public class WindowDescriptor
    {
        public IWindowComponent Logic { get; private set; }
        public WindowView View { get; private set; }

        public WindowDescriptor([NotNull] IWindowComponent logic, [NotNull] WindowView view)
        {
            if (logic == null) throw new ArgumentNullException("logic");
            if (view == null) throw new ArgumentNullException("view");

            Logic = logic;
            View = view;
        }
    }
}
