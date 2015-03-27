using System;
using System.Linq;
using System.Reflection;
using ReeperCommon.Extensions;
using ReeperCommon.FileSystem;
using ReeperCommon.FileSystem.Implementations;

namespace AssemblyReloader.Queries.FileSystemQueries
{
    class AssemblyDirectoryQuery : IAssemblyDirectoryQuery
    {
        private readonly Assembly _assembly;
        private readonly IDirectory _gameData;

        private IDirectory _location;

        public AssemblyDirectoryQuery(Assembly assembly, IDirectory gameData)
        {
            if (assembly == null) throw new ArgumentNullException("assembly");
            if (gameData == null) throw new ArgumentNullException("gameData");
            if (string.IsNullOrEmpty(assembly.CodeBase))
                throw new InvalidOperationException("Assembly must have been loaded from disk");

            _assembly = assembly;
            _gameData = gameData;
        }


        public IDirectory Get()
        {
            LazyInitialize();

            return _location;
        }

        private void LazyInitialize()
        {
            if (!_location.IsNull()) return;

            var loaded = AssemblyLoader.loadedAssemblies.FirstOrDefault(la => ReferenceEquals(la.assembly, _assembly));
            if (loaded.IsNull()) return;

            var possible = _gameData.Directory(new KSPUrlIdentifier(loaded.url));

            if (possible.Any()) _location = possible.Single();
        }
    }
}
