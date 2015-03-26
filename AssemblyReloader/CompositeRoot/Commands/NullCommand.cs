using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssemblyReloader.CompositeRoot.Commands
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
