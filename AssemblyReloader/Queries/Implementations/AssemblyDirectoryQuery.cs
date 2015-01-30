using System;
using System.Linq;
using System.Reflection;
using AssemblyReloader.Providers;
using ReeperCommon.Extensions.Object;
using ReeperCommon.FileSystem;
using ReeperCommon.Logging;

namespace AssemblyReloader.Queries.Implementations
{
    class AssemblyDirectoryQuery : IAssemblyDirectoryQuery
    {
        private readonly Assembly _assembly;
        private readonly IDirectory _gameData;
        private readonly ILog _log;

        private IDirectory _location;

        public AssemblyDirectoryQuery(Assembly assembly, IDirectory gameData, ILog log)
        {
            if (assembly == null) throw new ArgumentNullException("assembly");
            if (gameData == null) throw new ArgumentNullException("gameData");
            if (log == null) throw new ArgumentNullException("log");
            if (string.IsNullOrEmpty(assembly.CodeBase))
                throw new InvalidOperationException("Assembly must have been loaded from disk");

            _assembly = assembly;
            _gameData = gameData;
            _log = log;
        }


        public IDirectory Get()
        {
            _log.Normal("ADP: get");
            LazyInitialize();

            return _location;
        }

        private void LazyInitialize()
        {
            if (!_location.IsNull()) return;

            var loaded = AssemblyLoader.loadedAssemblies.FirstOrDefault(la => ReferenceEquals(la.assembly, _assembly));
            if (loaded.IsNull()) return;

            var possible = _gameData.Directory(loaded.url);

            if (possible.Any()) _location = possible.Single();
        }
    }
}
