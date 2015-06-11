using System;
using AssemblyReloader.Annotations;
using AssemblyReloader.Controllers;

namespace AssemblyReloader.Messages
{
    public class ToggleOptionsWindow : IViewMessage
    {
        public IReloadablePlugin Plugin { get; private set; }

        public ToggleOptionsWindow([NotNull] IReloadablePlugin plugin)
        {
            if (plugin == null) throw new ArgumentNullException("plugin");

            Plugin = plugin;
        }
    }
}
