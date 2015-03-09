using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AssemblyReloader.Game;

namespace AssemblyReloader.CompositeRoot.Commands
{
    public class RefreshPartActionWindows : ICommand
    {
        private readonly IPartActionWindowController _controller;

        public RefreshPartActionWindows(IPartActionWindowController controller)
        {
            if (controller == null) throw new ArgumentNullException("controller");
            _controller = controller;
        }


        public void Execute()
        {
            _controller.Refresh();
        }
    }
}
