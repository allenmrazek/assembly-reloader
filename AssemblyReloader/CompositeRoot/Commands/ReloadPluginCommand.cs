using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AssemblyReloader.PluginTracking;

namespace AssemblyReloader.CompositeRoot.Commands
{
    public class ReloadPluginCommand : ICommand<IReloadablePlugin>
    {
        public void Execute(IReloadablePlugin context)
        {
            context.Unload();
            context.Load();
        }
    }
}
