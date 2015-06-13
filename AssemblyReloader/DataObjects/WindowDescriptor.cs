using System;
using AssemblyReloader.Annotations;
using ReeperCommon.Gui.Window;
using ReeperCommon.Gui.Window.View;

namespace AssemblyReloader.DataObjects
{
    public class WindowDescriptor
    {
        public IWindowComponent BaseLogic { get; private set; }
        public IWindowComponent DecoratedLogic { get; private set; }
        public WindowView View { get; private set; }

        public WindowDescriptor([NotNull] IWindowComponent baseLogic, [NotNull] IWindowComponent decoratedLogic, [NotNull] WindowView view)
        {
            if (baseLogic == null) throw new ArgumentNullException("baseLogic");
            if (decoratedLogic == null) throw new ArgumentNullException("decoratedLogic");
            if (view == null) throw new ArgumentNullException("view");

            BaseLogic = baseLogic;
            DecoratedLogic = decoratedLogic;
            View = view;
        }
    }
}
