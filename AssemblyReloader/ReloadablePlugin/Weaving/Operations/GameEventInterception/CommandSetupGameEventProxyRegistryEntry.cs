using System;
using AssemblyReloader.Game;
using ReeperAssemblyLibrary;
using ReeperCommon.Logging;
using strange.extensions.command.impl;

namespace AssemblyReloader.ReloadablePlugin.Weaving.Operations.GameEventInterception
{
    public class CommandSetupGameEventProxyRegistryEntry : Command
    {
        private readonly ILoadedAssemblyHandle _handle;
        private readonly IGameEventProxy _gameEventProxy;
        private readonly IGameEventRegistry _gameEventRegistry;
        private readonly ILog _log;

        public CommandSetupGameEventProxyRegistryEntry(
            ILoadedAssemblyHandle handle,
            IGameEventProxy gameEventProxy,
            IGameEventRegistry gameEventRegistry,
            ILog log)
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
            _log.Debug("Creating GameEvent proxy registry entry");

            _gameEventProxy.AddRegistry(_handle, _gameEventRegistry);
        }
    }
}
