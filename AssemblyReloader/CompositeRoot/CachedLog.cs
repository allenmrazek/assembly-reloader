using System;
using System.Collections.Generic;
using System.Linq;
using ReeperCommon.Logging;

namespace AssemblyReloader.CompositeRoot
{
    /// <summary>I
    /// Essentially a decorator that caches log messages for display in the view
    /// </summary>
    class CachedLog : ICachedLog
    {
        private readonly ILog _mainLog;
        private readonly Queue<string> _messages = new Queue<string>();
        private readonly int _messageBuffer = 100;



        public CachedLog(ILog mainBaseLog, int buffer)
        {
            if (mainBaseLog == null) throw new ArgumentNullException("mainBaseLog");

            _mainLog = mainBaseLog;
            _messageBuffer = buffer;
        }



        private void AddEntry(string entry)
        {
            if (_messages.Count >= _messageBuffer)
                _messages.Dequeue();

            _messages.Enqueue(entry);
        }



        public void Debug(string format, params string[] args)
        {
            var msg = string.Format("[DBG] " + format, args);
            AddEntry(msg);
            _mainLog.Debug(format, args);
        }



        public void Normal(string format, params string[] args)
        {
            var msg = string.Format(format, args);
            AddEntry(msg);
            _mainLog.Normal(format, args);
        }



        public void Warning(string format, params string[] args)
        {
            var msg = string.Format("[WRN] " + format, args);
            AddEntry(msg);
            _mainLog.Warning(format, args);
        }



        public void Error(string format, params string[] args)
        {
            var msg = string.Format("[ERR] " + format, args);
            AddEntry(msg);
            _mainLog.Error(format, args);
        }



        public void Performance(string format, params string[] args)
        {
            var msg = string.Format("[PERF] " + format, args);
            AddEntry(msg);
            _mainLog.Performance(format, args);
        }



        public void Verbose(string format, params string[] args)
        {
            var msg = string.Format(format, args);
            AddEntry(msg);
            _mainLog.Verbose(format, args);
        }



        public ILog CreateTag(string tag)
        {
            return new CachedLog(new TaggedLog(this, tag), _messageBuffer);
        }



        public string[] Messages { get { return _messages.ToArray().Reverse().ToArray(); } }
    }
}
