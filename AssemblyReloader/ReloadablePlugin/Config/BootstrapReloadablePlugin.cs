using System;
using AssemblyReloader.StrangeIoC.extensions.context.impl;
using ReeperCommon.FileSystem;

namespace AssemblyReloader.ReloadablePlugin.Config
{
    public class BootstrapReloadablePlugin : ContextView
    {
        public void Bootstrap(IFile file)
        {
            if (file == null) throw new ArgumentNullException("file");

            context = new ReloadablePluginContext(this, file);
            context.Start();
        }
    }
}
