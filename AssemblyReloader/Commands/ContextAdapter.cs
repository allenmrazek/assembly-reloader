using System;
using AssemblyReloader.Annotations;

namespace AssemblyReloader.Commands
{
    public class ContextAdapter<TContextFrom, TContextTo> : ICommand<TContextFrom>
    {
        private readonly Func<TContextFrom, TContextTo> _contextAdapter;
        private readonly ICommand<TContextTo> _adaptedCommand;

        public ContextAdapter(
            [NotNull] Func<TContextFrom, TContextTo> contextAdapter,
            [NotNull] ICommand<TContextTo> adaptedCommand)
        {
            if (contextAdapter == null) throw new ArgumentNullException("contextAdapter");
            if (adaptedCommand == null) throw new ArgumentNullException("adaptedCommand");
            _contextAdapter = contextAdapter;
            _adaptedCommand = adaptedCommand;
        }


        public void Execute(TContextFrom context)
        {
            var nextContext = _contextAdapter(context);

            _adaptedCommand.Execute(nextContext);
        }
    }
}
