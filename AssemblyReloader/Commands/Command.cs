using System;
using System.Runtime.Remoting.Contexts;
using AssemblyReloader.Properties;

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


    public class Command<TContext> : ICommand<TContext>
    {
        private readonly Action<TContext> _action;

        public Command([NotNull] Action<TContext> action)
        {
            if (action == null) throw new ArgumentNullException("action");
            _action = action;
        }

        public void Execute(TContext context)
        {
            _action(context);
        }
    }
}
