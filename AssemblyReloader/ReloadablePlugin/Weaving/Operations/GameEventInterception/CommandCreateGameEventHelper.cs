extern alias KSP;
using System;
using System.Linq;
using Mono.Cecil;
using ReeperCommon.Logging;
using strange.extensions.command.impl;
using GameEvents = KSP::GameEvents;

namespace AssemblyReloader.ReloadablePlugin.Weaving.Operations.GameEventInterception
{
    public class CommandCreateGameEventHelper : Command
    {
        private readonly AssemblyDefinition _context;
        private readonly TypeDefinition _helper;
        private readonly ILog _log;

        public CommandCreateGameEventHelper(AssemblyDefinition context, TypeDefinition helper, ILog log)
        {
            if (context == null) throw new ArgumentNullException("context");
            if (helper == null) throw new ArgumentNullException("helper");
            if (log == null) throw new ArgumentNullException("log");

            _context = context;
            _helper = helper;
            _log = log;
        }


        public override void Execute()
        {
            _log.Normal("Creating GameEvent helper");

            //var fieldDefinition = new FieldDefinition("StaticField", FieldAttributes.Static | FieldAttributes.Public,
            //    _helper.Module.Import(typeof (KSP::EventData<Vessel>)));

            //_helper.Fields.Add(fieldDefinition);

            var nested = typeof (GameEvents).GetNestedTypes().Union(new[] {typeof (GameEvents)}).ToList();

            nested.ForEach(nt => _log.Normal("Nested type: " + nt.FullName));
        }
    }
}
