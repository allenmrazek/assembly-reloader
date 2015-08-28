using System;
using ReeperCommon.FileSystem;
using strange.extensions.context.impl;

namespace AssemblyReloader.ReloadablePlugin.Config
{
// ReSharper disable once ClassNeverInstantiated.Global
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
