using System;
using AssemblyReloader.Annotations;

namespace AssemblyReloader.Commands
{
    public class Command : ICommand
    {
        private readonly Action _action;

        public Command([NotNull] Action action)
        {
            if (action == null) throw new ArgumentNullException("action");
            _action = action;
        }


        public void Execute()
        {
            _action();
        }
    }
}
