using System;
using AssemblyReloader.Commands;

namespace AssemblyReloader.Game.Commands
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
