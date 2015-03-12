using System;
using System.Linq;
using System.Reflection;
using AssemblyReloader.Disk;
using AssemblyReloader.Providers;
using AssemblyReloader.Weaving;
using ReeperCommon.Containers;
using ReeperCommon.Extensions;

namespace AssemblyReloader.Game
{
    public class KspAssemblyLoader : IAssemblyLoader
    {
        private readonly IAssemblyDefinitionReader _definitionReader;
        private readonly IAssemblyProvider _assemblyFromDefinitionProvider;
        private readonly ILoadedAssemblyFactory _laFactory;
        private readonly IAssemblyDefinitionWeaver _weaver;
        private readonly bool _writeResultToDisk;
        private IDisposable _loadedAssembly;

        public KspAssemblyLoader(
            IAssemblyDefinitionReader definitionReader,
            IAssemblyProvider assemblyFromDefinitionProvider,
            ILoadedAssemblyFactory laFactory,
            IAssemblyDefinitionWeaver weaver,
            bool writeResultToDisk)
        {
            if (definitionReader == null) throw new ArgumentNullException("definitionReader");
            if (assemblyFromDefinitionProvider == null) throw new ArgumentNullException("assemblyFromDefinitionProvider");
            if (laFactory == null) throw new ArgumentNullException("laFactory");
            if (weaver == null) throw new ArgumentNullException("weaver");

            _definitionReader = definitionReader;
            _assemblyFromDefinitionProvider = assemblyFromDefinitionProvider;
            _laFactory = laFactory;
            _weaver = weaver;
            _writeResultToDisk = writeResultToDisk;
        }


        public Maybe<Assembly> Load()
        {
            if (!_loadedAssembly.IsNull())
                throw new InvalidOperationException(_definitionReader.Name + " has not been unloaded");

            var assemblyDefinition = _definitionReader.Get();

            if (!assemblyDefinition.Any())
                throw new Exception("Failed to read " + _definitionReader.Name + " definition");

            

            if (!_weaver.Weave(assemblyDefinition.Single()))
                throw new Exception("Failed to reweave " + _definitionReader.Name + " il");

            // try to write first, in case loading it from memory fails (usually due to an error
            // in rewritten il code)

            if (_writeResultToDisk)
                assemblyDefinition.Single().Write(_definitionReader.Location.FullPath + ".debug");

            var result = _assemblyFromDefinitionProvider.Get(assemblyDefinition.Single());

            if (!result.Any()) return Maybe<Assembly>.None;



            _loadedAssembly = _laFactory.Create(result.Single(), _definitionReader.Location);

            return result;
        }


        public void Unload()
        {
            if (!_loadedAssembly.IsNull())
                _loadedAssembly.Dispose();

            _loadedAssembly = null;
        }
    }
}
