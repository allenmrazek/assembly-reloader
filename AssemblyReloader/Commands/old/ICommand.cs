namespace AssemblyReloader.Commands.old
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
