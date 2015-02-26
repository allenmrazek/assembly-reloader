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

    public class CompositeCommand<TContext> : ICommand<TContext>
    {
        private readonly ICommand<TContext>[] _commands;

        public CompositeCommand(params ICommand<TContext>[] commands)
        {
            if (commands == null) throw new ArgumentNullException("commands");
            _commands = commands;
        }

        public void Execute(TContext context)
        {
            foreach (var c in _commands)
                c.Execute(context);
        }
    }
}
