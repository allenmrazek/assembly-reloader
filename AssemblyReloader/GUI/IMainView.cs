using System.Collections.Generic;
using ReeperCommon.Gui.Window;

namespace AssemblyReloader.Gui
{
    public interface IMainView : IWindowComponent
    {
        IEnumerable<IPluginInfo> Plugins { get; set; }

        void ToggleSettings();
        void Close();
    }
}
