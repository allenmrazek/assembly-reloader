using System.Collections.Generic;
using AssemblyReloader.Config;
using AssemblyReloader.DataObjects;

namespace AssemblyReloader.ReloadablePlugin.Definition.Operations
{
    public interface IWeaveOperationFactory
    {
        IEnumerable<IWeaveOperation> Create(PluginConfiguration pluginConfiguration);
    }
}
