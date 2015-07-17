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


        public AssemblyDefinitionWeaver(IAssemblyDefinitionReader definitionReader, [Name(LogKeys.PluginContext)] ILog log)
        {
            if (definitionReader == null) throw new ArgumentNullException("definitionReader");
            if (log == null) throw new ArgumentNullException("log");

            _log = log;
            _definitionReader = definitionReader;
        }


        public Maybe<AssemblyDefinition> Read()
        {
            var unmodifiedDefinition = _definitionReader.Read();

            if (!unmodifiedDefinition.Any())
                return Maybe<AssemblyDefinition>.None;

            _log.Debug("Weaving definition of {0}", unmodifiedDefinition.Single().FullName);

            // todo: weave
            return Maybe<AssemblyDefinition>.None;
        }
    }
}
