namespace AssemblyReloader.Commands
{
    public interface ICommand
    {
        void Execute();
    }

    public interface ICommand<TContext>
    {
        void Execute(TContext context);
    }
}
