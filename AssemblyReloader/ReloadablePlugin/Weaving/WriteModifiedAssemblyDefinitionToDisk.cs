extern alias Cecil96;
using System;
using System.IO;
using System.Reflection;
using ReeperCommon.Containers;
using ReeperCommon.FileSystem;
using ReeperCommon.Logging;
using AssemblyDefinition = Cecil96::Mono.Cecil.AssemblyDefinition;

namespace AssemblyReloader.ReloadablePlugin.Weaving
{
    public class WriteModifiedAssemblyDefinitionToDisk : IAssemblyDefinitionLoader
    {
        private readonly IAssemblyDefinitionLoader _decorated;
        private readonly IDirectory _dumpDirectory;
        private readonly Func<bool> _condition;
        private readonly ILog _log;

        public WriteModifiedAssemblyDefinitionToDisk(
            IAssemblyDefinitionLoader decorated,
            IDirectory dumpDirectory,
            Func<bool> condition,
            ILog log)
        {
            if (decorated == null) throw new ArgumentNullException("decorated");
            if (dumpDirectory == null) throw new ArgumentNullException("dumpDirectory");
            if (condition == null) throw new ArgumentNullException("condition");
            if (log == null) throw new ArgumentNullException("log");

            _decorated = decorated;
            _dumpDirectory = dumpDirectory;
            _condition = condition;
            _log = log;
        }


        private void WriteToDisk(AssemblyDefinition definition)
        {
            if (definition == null) throw new ArgumentNullException("definition");

            var filename = Path.Combine(_dumpDirectory.FullPath, definition.MainModule.Name + ".dump");

            _log.Normal("Saving rewritten assembly to \"" + filename + "\"");
            definition.Write(filename);
        }


        public Maybe<Assembly> Get(AssemblyDefinition definition)
        {
            if (definition == null) throw new ArgumentNullException("definition");

            if (_condition())
                WriteToDisk(definition);

            return _decorated.Get(definition);
        }
    }
}
