using System;
using System.Reflection;
using ReeperCommon.Logging;
using strange.extensions.command.impl;
using UnityEngine;

namespace AssemblyReloader.ReloadablePlugin
{
    public class CommandSendReloadRequestedMessageToTarget : Command
    {
        public const string MessageMethodName = "OnPluginReloadRequested";

        private readonly MonoBehaviour _context;
        private readonly ILog _log;


        public CommandSendReloadRequestedMessageToTarget(MonoBehaviour context, ILog log)
        {
            if (context == null) throw new ArgumentNullException("context");
            if (log == null) throw new ArgumentNullException("log");

            _context = context;
            _log = log;
        }


        public override void Execute()
        {
            if (_context == null)
            {
                _log.Warning("Can't send message to target MonoBehaviour because it is null");
                return;
            }

            // note: we use reflection rather than SendMessage here because SendMessage will fail
            // if the target component is inactive
            var method = _context.GetType().GetMethod(MessageMethodName,
                  BindingFlags.Instance | BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.NonPublic,
                  null, new Type[] { }, null);

            if (method == null) return; // having no such method is valid; simply do nothing

            _log.Verbose("Sending " + MessageMethodName + " to target " + _context.name + ", " +
                         _context.GetType().FullName);

            // ReSharper disable once CoVariantArrayConversion
            method.Invoke(_context, Type.EmptyTypes);
        }
    }
}
