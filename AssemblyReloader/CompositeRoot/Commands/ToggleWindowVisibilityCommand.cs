using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReeperCommon.Gui.Window;
using ReeperCommon.Logging.Implementations;

namespace AssemblyReloader.CompositeRoot.Commands
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

            new DebugLog().Normal("ToggleWindowVisibilityCommand.Execute: " + _window.Visible);
        }
    }
}
