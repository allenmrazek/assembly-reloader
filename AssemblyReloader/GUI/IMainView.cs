using System.Collections.Generic;

namespace AssemblyReloader.Gui
{
    public interface IMainView
    {
        IEnumerable<IPluginInfo> Plugins { get; set; }
    }
}
