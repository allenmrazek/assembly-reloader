using System;
using AssemblyReloader.Properties;

namespace AssemblyReloader.Commands
{
    public class ContextProviderCommand<TContext> : ICommand
    {
        private readonly Func<TContext> _contextProvider;
        private readonly ICommand<TContext> _command;


        public ContextProviderCommand(
            [NotNull] Func<TContext> contextProvider,
            [NotNull] ICommand<TContext> command)
        {
            if (contextProvider == null) throw new ArgumentNullException("contextProvider");
            if (command == null) throw new ArgumentNullException("command");
            _contextProvider = contextProvider;
            _command = command;
        }


        public void Execute()
        {
            _command.Execute(_contextProvider());
        }
    }
}
