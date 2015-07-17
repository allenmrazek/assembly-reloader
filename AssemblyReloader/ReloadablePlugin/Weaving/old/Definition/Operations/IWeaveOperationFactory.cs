using System.Collections.Generic;
using AssemblyReloader.Config;

namespace AssemblyReloader.ReloadablePlugin.Weaving.old.Definition.Operations
{
    public interface IWeaveOperationFactory
    {
        IEnumerable<IWeaveOperation> Create(PluginConfiguration pluginConfiguration);
    }
}
