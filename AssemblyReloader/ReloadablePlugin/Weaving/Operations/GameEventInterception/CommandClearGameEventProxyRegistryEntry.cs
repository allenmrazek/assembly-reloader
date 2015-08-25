using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AssemblyReloader.Game;
using ReeperCommon.Logging;
using strange.extensions.command.impl;

namespace AssemblyReloader.ReloadablePlugin.Weaving.Operations.GameEventInterception
{
    public class CommandClearGameEventProxyRegistryEntry : Command
    {
        private readonly ILoadedAssemblyHandle _handle;
        private readonly IGameEventProxy _gameEventProxy;
        private readonly IGameEventRegistry _gameEventRegistry;
        private readonly ILog _log;

        public CommandClearGameEventProxyRegistryEntry(
            ILoadedAssemblyHandle handle, 
            IGameEventProxy gameEventProxy,
            IGameEventRegistry gameEventRegistry, ILog log)
        {
            if (handle == null) throw new ArgumentNullException("handle");
            if (gameEventProxy == null) throw new ArgumentNullException("gameEventProxy");
            if (gameEventRegistry == null) throw new ArgumentNullException("gameEventRegistry");
            if (log == null) throw new ArgumentNullException("log");

            _handle = handle;
            _gameEventProxy = gameEventProxy;
            _gameEventRegistry = gameEventRegistry;
            _log = log;
        }


        public override void Execute()
        {
            _log.Verbose("Removing game event proxy registry");

            foreach (var callback in _gameEventRegistry)
                _log.Warning(string.Format("{0} was not removed", callback));

            _gameEventRegistry.ClearCallbacks();
            _gameEventProxy.RemoveRegistry(_handle);
        }
    }
}
