using System;
using AssemblyReloader.Annotations;
using AssemblyReloader.Controllers;

namespace AssemblyReloader.Gui.Messages
{
    public class TogglePluginOptionsWindow : IViewMessage
    {
        public IReloadablePlugin Plugin { get; private set; }

        public TogglePluginOptionsWindow([NotNull] IReloadablePlugin plugin)
        {
            if (plugin == null) throw new ArgumentNullException("plugin");

            Plugin = plugin;
        }
    }
}
