using System;
using System.Linq;
using AssemblyReloader.Config.Keys;
using AssemblyReloader.StrangeIoC.extensions.injector;
using Mono.Cecil;
using ReeperCommon.Containers;
using ReeperCommon.FileSystem;
using ReeperCommon.Logging;

namespace AssemblyReloader.ReloadablePlugin.Weaving
{
    public class AssemblyDefinitionWeaver : IAssemblyDefinitionReader
    {
        private readonly ILog _log;
        private readonly IAssemblyDefinitionReader _definitionReader;
        private readonly SignalDefinitionReady _definitionReadySignal;


        public AssemblyDefinitionWeaver(
            IAssemblyDefinitionReader definitionReader, 
            SignalDefinitionReady definitionReadySignal,
            [Name(LogKeys.PluginContext)] ILog log)
        {
            if (definitionReader == null) throw new ArgumentNullException("definitionReader");
            if (definitionReadySignal == null) throw new ArgumentNullException("definitionReadySignal");
            if (log == null) throw new ArgumentNullException("log");

            _log = log;
            _definitionReader = definitionReader;
            _definitionReadySignal = definitionReadySignal;
        }


        public Maybe<AssemblyDefinition> Read()
        {
            var unmodifiedDefinition = _definitionReader.Read();

            if (!unmodifiedDefinition.Any())
                return Maybe<AssemblyDefinition>.None;

            var weavedDefinition = unmodifiedDefinition.Single();

            _log.Debug("Weaving definition of {0}", weavedDefinition.FullName);
            _definitionReadySignal.Dispatch(weavedDefinition);

            return Maybe<AssemblyDefinition>.With(weavedDefinition);
        }
    }
}
