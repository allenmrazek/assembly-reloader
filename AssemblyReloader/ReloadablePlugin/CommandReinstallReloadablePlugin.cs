using System;
using System.Linq;
using ReeperAssemblyLibrary;
using ReeperCommon.Containers;
using ReeperCommon.Logging;
using strange.extensions.command.impl;

namespace AssemblyReloader.ReloadablePlugin
{
    /// <summary>
    /// If the GameDatabase is reloaded, all AssemblyLoader references to reloadable plugins will
    /// be removed. We'll need to put them back
    /// </summary>
    public class CommandReinstallReloadablePlugin : Command
    {
        private readonly Maybe<ILoadedAssemblyHandle> _handle;
        private readonly ILog _log;

        public CommandReinstallReloadablePlugin(
            Maybe<ILoadedAssemblyHandle> handle,
            ILog log)
        {
            if (log == null) throw new ArgumentNullException("log");

            _handle = handle;
            _log = log;
        }

        public override void Execute()
        {
            if (!_handle.Any())
                return;

            if (AssemblyLoader.loadedAssemblies.Any(la => la.dllName == _handle.Single().LoadedAssembly.dllName))
            {
                _log.Error("AssemblyLoader already has a reference to a LoadedAssembly " +
                           _handle.Single().LoadedAssembly.dllName);
            }
            else
            {
                _handle
                    .With(h => h.LoadedAssembly)
                    .Do(la =>
                    {
                        _log.Normal("Reinstalling " + _handle.Single().LoadedAssembly.dllName +
                                    " into AssemblyLoader due to GameDatabase reload");
                        AssemblyLoader.loadedAssemblies.Add(la);
                    });
            }
        }
    }
}
