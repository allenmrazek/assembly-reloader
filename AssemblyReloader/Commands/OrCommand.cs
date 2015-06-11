using System;
using AssemblyReloader.Annotations;

namespace AssemblyReloader.Commands
{
    public class OrCommand : ICommand
    {
        private readonly Func<bool> _condition;
        private readonly ICommand _executeIfTrue;
        private readonly ICommand _executeIfFalse;

        public OrCommand(
            [NotNull] Func<bool> condition, 
            [NotNull] ICommand executeIfTrue, 
            [NotNull] ICommand executeIfFalse)
        {
            if (condition == null) throw new ArgumentNullException("condition");
            if (executeIfTrue == null) throw new ArgumentNullException("executeIfTrue");
            if (executeIfFalse == null) throw new ArgumentNullException("executeIfFalse");

            _condition = condition;
            _executeIfTrue = executeIfTrue;
            _executeIfFalse = executeIfFalse;
        }


        public void Execute()
        {
            var command = _condition() ? _executeIfTrue : _executeIfFalse;

            command.Execute();
        }
    }
}
