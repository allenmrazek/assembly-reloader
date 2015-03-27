namespace AssemblyReloader.Commands
{
    public class NullCommand : ICommand
    {
        public void Execute()
        {
            
        }
    }

    public class NullCommand<T> : ICommand<T>
    {
        public void Execute(T context)
        {
            
        }
    }
}
