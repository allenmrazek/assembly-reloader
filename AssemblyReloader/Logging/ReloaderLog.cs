using System;
using System.Collections.Generic;
using ReeperCommon.Logging;
using ReeperCommon.Logging.Implementations;

namespace AssemblyReloader.Logging
{
    /// <summary>
    /// Essentially a decorator that caches log messages for display in the view
    /// </summary>
    class ReloaderLog : StandardLog
    {
        private readonly Log _main;
        private readonly Queue<string> _messages = new Queue<string>();
        private readonly int _messageBuffer = 100;



        public ReloaderLog(Log mainLog, int buffer = 50)
        {
            if (mainLog == null) throw new ArgumentNullException("mainLog");

            _main = mainLog;
            _messageBuffer = buffer;
        }



        private void AddEntry(string entry)
        {
            if (_messages.Count >= _messageBuffer)
                _messages.Dequeue();

            _messages.Enqueue(entry);
        }



        public override void Debug(string format, params string[] args)
        {
            var msg = string.Format("[DBG] " + format, args);
            AddEntry(msg);
            base.Debug(format, args);
        }



        public override void Normal(string format, params string[] args)
        {
            var msg = string.Format(format, args);
            AddEntry(msg);
            base.Normal(format, args);
        }



        public override void Warning(string format, params string[] args)
        {
            var msg = string.Format("[WRN] " + format, args);
            AddEntry(msg);
            base.Warning(format, args);
        }



        public override void Error(string format, params string[] args)
        {
            var msg = string.Format("[ERR] " + format, args);
            AddEntry(msg);
            base.Error(format, args);
        }



        public override void Performance(string format, params string[] args)
        {
            var msg = string.Format("[PERF] " + format, args);
            AddEntry(msg);
            base.Performance(format, args);
        }



        public override void Verbose(string format, params string[] args)
        {
            var msg = string.Format(format, args);
            AddEntry(msg);
            base.Verbose(format, args);
        }
    }
}
