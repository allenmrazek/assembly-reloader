using System.Collections.Generic;
using AssemblyReloader.Config;
using AssemblyReloader.Gui;

namespace AssemblyReloader.DataObjects
{
    public interface IModel
    {
        IEnumerable<IPluginInfo> Plugins { get; }
        Configuration Configuration { get; }

        bool Reload(IPluginInfo plugin);
    }
}
