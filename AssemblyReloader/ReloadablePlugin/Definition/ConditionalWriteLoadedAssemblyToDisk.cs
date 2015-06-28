using System;
using System.IO;
using System.Linq;
using AssemblyReloader.Properties;
using ReeperCommon.Containers;
using ReeperCommon.FileSystem;

namespace AssemblyReloader.ReloadablePlugin.Definition
{
    public class ConditionalWriteLoadedAssemblyToDisk : IAssemblyDefinitionProvider
    {
        private readonly IAssemblyDefinitionProvider _decorated;
        private readonly Func<bool> _condition;

        public ConditionalWriteLoadedAssemblyToDisk(
            [NotNull] IAssemblyDefinitionProvider decorated,
            [NotNull] Func<bool> condition)
        {
            if (decorated == null) throw new ArgumentNullException("decorated");
            if (condition == null) throw new ArgumentNullException("condition");

            _decorated = decorated;
            _condition = condition;
        }


        public Maybe<Mono.Cecil.AssemblyDefinition> Get([NotNull] IFile location)
        {
            if (location == null) throw new ArgumentNullException("location");

            var definition = _decorated.Get(location);

            if (definition.Any() && _condition()) WriteToDisk(definition.Single(), location.Directory);

            return definition;
        }


        private static void WriteToDisk([NotNull] Mono.Cecil.AssemblyDefinition definition, [NotNull] IDirectory directory)
        {
            if (definition == null) throw new ArgumentNullException("definition");
            if (directory == null) throw new ArgumentNullException("directory");

            var filename = Path.Combine(directory.FullPath, definition.MainModule.Name + ".dump");

            definition.Write(filename);
        }
    }
}
