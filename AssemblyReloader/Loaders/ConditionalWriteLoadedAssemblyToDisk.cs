using System;
using System.IO;
using System.Linq;
using System.Reflection;
using AssemblyReloader.Annotations;
using Mono.Cecil;
using ReeperCommon.Containers;
using ReeperCommon.FileSystem;

namespace AssemblyReloader.Loaders
{
    public class ConditionalWriteLoadedAssemblyToDisk : IAssemblyDefinitionLoader
    {
        private readonly IAssemblyDefinitionLoader _loader;
        private readonly Func<bool> _condition;
        private readonly IDirectory _location;

        public ConditionalWriteLoadedAssemblyToDisk([NotNull] IAssemblyDefinitionLoader loader,
            [NotNull] Func<bool> condition, [NotNull] IDirectory location)
        {
            if (loader == null) throw new ArgumentNullException("loader");
            if (condition == null) throw new ArgumentNullException("condition");
            if (location == null) throw new ArgumentNullException("location");

            _loader = loader;
            _condition = condition;
            _location = location;
        }


        public Maybe<Assembly> LoadDefinition(AssemblyDefinition definition)
        {
            var result = _loader.LoadDefinition(definition);

            if (result.Any() && _condition()) WriteToDisk(definition);

            return result;
        }


        private void WriteToDisk(AssemblyDefinition definition)
        {
            var filename = Path.Combine(_location.FullPath, definition.MainModule.Name + ".dump");

            definition.Write(filename);
        }
    }
}
