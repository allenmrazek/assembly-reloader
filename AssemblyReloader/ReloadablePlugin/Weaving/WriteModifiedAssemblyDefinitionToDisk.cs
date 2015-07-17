using System;
using System.IO;
using System.Linq;
using System.Reflection;
using AssemblyReloader.Properties;
using Mono.Cecil;
using ReeperCommon.Containers;
using ReeperCommon.FileSystem;

namespace AssemblyReloader.ReloadablePlugin.Weaving
{
    public class WriteModifiedAssemblyDefinitionToDisk : IAssemblyDefinitionLoader
    {
        private readonly IAssemblyDefinitionLoader _decorated;
        private readonly IDirectory _dumpDirectory;
        private readonly Func<bool> _condition;

        public WriteModifiedAssemblyDefinitionToDisk(
            [NotNull] IAssemblyDefinitionLoader decorated,
            IDirectory dumpDirectory,
            [NotNull] Func<bool> condition)
        {
            if (decorated == null) throw new ArgumentNullException("decorated");
            if (dumpDirectory == null) throw new ArgumentNullException("dumpDirectory");
            if (condition == null) throw new ArgumentNullException("condition");

            _decorated = decorated;
            _dumpDirectory = dumpDirectory;
            _condition = condition;
        }


        private void WriteToDisk([NotNull] AssemblyDefinition definition)
        {
            if (definition == null) throw new ArgumentNullException("definition");

            var filename = Path.Combine(_dumpDirectory.FullPath, definition.MainModule.Name + ".dump");

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
