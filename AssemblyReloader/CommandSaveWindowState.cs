extern alias KSP;
using System;
using ReeperCommon.Gui.Window;
using ReeperCommon.Logging;
using ReeperCommon.Serialization;
using strange.extensions.command.impl;

namespace AssemblyReloader
{
    public class CommandSaveWindowState : Command
    {
        private readonly IWindowComponent _window;
        private readonly IConfigNodeSerializer _serializer;
        private readonly ILog _log;

        public CommandSaveWindowState(
            IWindowComponent window, 
            IConfigNodeSerializer serializer,
            ILog log)
        {
            if (window == null) throw new ArgumentNullException("window");
            if (serializer == null) throw new ArgumentNullException("serializer");
            if (log == null) throw new ArgumentNullException("log");

            _window = window;
            _serializer = serializer;
            _log = log;
        }

        public override void Execute()
        {
            _log.Normal("Command Save window: " + _window.Title);

            var cfg = new KSP::ConfigNode("TestNode");

            _serializer.Serialize(_window, cfg);

            _log.Normal("SerializeD: {0}", cfg.ToString());
        }
    }
}
