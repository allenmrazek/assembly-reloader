using System;

namespace AssemblyReloader.CompositeRoot.Commands
{
    public class CompositeCommand : ICommand
    {
        private readonly ICommand[] _commands;

        public CompositeCommand(params ICommand[] commands)
        {
            if (commands == null) throw new ArgumentNullException("commands");
            _commands = commands;
        }

        public void Execute()
        {
            foreach (var c in _commands)
                c.Execute();
        }
    }
}
