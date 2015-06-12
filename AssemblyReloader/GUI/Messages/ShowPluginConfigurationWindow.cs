using System;
using AssemblyReloader.Annotations;

namespace AssemblyReloader.Gui.Messages
{
    public class ShowPluginConfigurationWindow
    {
        public IPluginInfo Plugin { get; private set; }

        public ShowPluginConfigurationWindow([NotNull] IPluginInfo plugin)
        {
            if (plugin == null) throw new ArgumentNullException("plugin");

            Plugin = plugin;
        }
    }
}
