extern alias Cecil96;
using System;
using System.Linq;
using AssemblyReloader.Gui;
using ReeperCommon.Logging;
using strange.extensions.command.impl;
using AssemblyDefinition = Cecil96::Mono.Cecil.AssemblyDefinition;

namespace AssemblyReloader.ReloadablePlugin.Weaving
{
// ReSharper disable once ClassNeverInstantiated.Global
    public class CommandReadAssemblyDefinition : Command
    {
        private readonly IPluginInfo _plugin;
        private readonly SignalPluginCannotBeLoaded _failSignal;
        private readonly IAssemblyDefinitionReader _definitionReader;
        private readonly ILog _log;


        public CommandReadAssemblyDefinition(
            IPluginInfo plugin,
            IAssemblyDefinitionReader definitionReader,
            SignalPluginCannotBeLoaded failSignal,
            ILog log)
        {
            if (plugin == null) throw new ArgumentNullException("plugin");
            if (definitionReader == null) throw new ArgumentNullException("definitionReader");
            if (failSignal == null) throw new ArgumentNullException("failSignal");
            if (log == null) throw new ArgumentNullException("log");
            _plugin = plugin;
            _definitionReader = definitionReader;
            _failSignal = failSignal;
            _log = log;
        }


        public override void Execute()
        {
            //injectionBinder.GetBinding<AssemblyDefinition>().Do(binding => injectionBinder.Unbind(binding));
            injectionBinder.Unbind<AssemblyDefinition>();

            var definition = _definitionReader.Read();

            if (!definition.Any())
            {
                _log.Error("Failed to read definition", _plugin.Location.Url);
                _failSignal.Dispatch("Failed to read " + _plugin.Name + " definition");
                Fail();
                return;
            }
            
            injectionBinder.Bind<AssemblyDefinition>().To(definition.Single());

            _log.Verbose("Successfully read definition");
        }
    }
}
