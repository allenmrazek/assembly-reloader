using System;
using ReeperCommon.Gui.Window;

namespace AssemblyReloader.Commands
{
    public class ToggleWindowVisibilityCommand : ICommand
    {
        private readonly IWindowComponent _window;

        public ToggleWindowVisibilityCommand(IWindowComponent window)
        {
            if (window == null) throw new ArgumentNullException("window");
            _window = window;
        }


        public void Execute()
        {
            _window.Visible = !_window.Visible;
        }
    }
}
