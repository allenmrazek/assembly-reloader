using System;
using AssemblyReloader.Annotations;

namespace AssemblyReloader.Commands
{
    public class ConditionalCommand : ICommand
    {
        private readonly ICommand _command;
        private readonly Func<bool> _condition;

        public ConditionalCommand([NotNull] ICommand command, [NotNull] Func<bool> condition)
        {
            if (command == null) throw new ArgumentNullException("command");
            if (condition == null) throw new ArgumentNullException("condition");
            _command = command;
            _condition = condition;
        }

        public void Execute()
        {
            if (_condition())
                _command.Execute();
        }
    }
}
