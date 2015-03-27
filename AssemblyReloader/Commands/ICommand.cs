namespace AssemblyReloader.Commands
{
    public interface ICommand
    {
        void Execute();
    }

    public interface ICommand<in TContext>
    {
        void Execute(TContext context);
    }
}
