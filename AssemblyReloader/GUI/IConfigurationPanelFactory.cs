using System.Collections.Generic;
using AssemblyReloader.DataObjects;

namespace AssemblyReloader.Gui
{
    public interface IConfigurationPanelFactory
    {
        IEnumerable<IExpandablePanel> CreatePanelsFor(Configuration configuration);
    }
}
