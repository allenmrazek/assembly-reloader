using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using AssemblyReloader.Game;
using ReeperAssemblyLibrary;
using ReeperCommon.Logging;
using strange.extensions.command.impl;

namespace AssemblyReloader.ReloadablePlugin.Weaving.Operations.GameEventInterception
{
    public class CommandClearGameEventProxyRegistryEntry : Command
    {
        private readonly ILoadedAssemblyHandle _handle;
        private readonly IGameEventProxy _gameEventProxy;
        private readonly IGameEventRegistry _gameEventRegistry;
        private readonly SignalPluginWasUnloaded _pluginWasUnloaded;
        private readonly ILog _log;

        public CommandClearGameEventProxyRegistryEntry(
            ILoadedAssemblyHandle handle, 
            IGameEventProxy gameEventProxy,
            IGameEventRegistry gameEventRegistry,
            SignalPluginWasUnloaded pluginWasUnloaded,
            ILog log)
        {
            if (handle == null) throw new ArgumentNullException("handle");
            if (gameEventProxy == null) throw new ArgumentNullException("gameEventProxy");
            if (gameEventRegistry == null) throw new ArgumentNullException("gameEventRegistry");
            if (pluginWasUnloaded == null) throw new ArgumentNullException("pluginWasUnloaded");
            if (log == null) throw new ArgumentNullException("log");

            _handle = handle;
            _gameEventProxy = gameEventProxy;
            _gameEventRegistry = gameEventRegistry;
            _pluginWasUnloaded = pluginWasUnloaded;
            _log = log;
        }


        public override void Execute()
        {
            _pluginWasUnloaded.AddOnce(PluginWasUnloaded);
            Retain();
        }


        private void PluginWasUnloaded()
        {
            _log.Verbose("Removing game event proxy registry");

            if (_gameEventRegistry.Count > 0)
                _log.Warning("Found {0} unremoved callbacks", _gameEventRegistry.Count.ToString(CultureInfo.InvariantCulture));

            foreach (var kvp in _gameEventRegistry)
                _log.Warning(string.Format("{0} {1} was not removed", kvp.Key, kvp.Value));

            _gameEventRegistry.ClearCallbacks();
            _gameEventProxy.RemoveRegistry(_handle);
            Release();
        }
    }
}
