using System;
using System.Linq;
using System.Reflection;
using AssemblyReloader.Game;
using AssemblyReloader.Properties;
using ReeperCommon.Extensions;
using ReeperCommon.FileSystem;

namespace AssemblyReloader.Queries.FileSystemQueries
{
    class GetAssemblyDirectory : IGetAssemblyDirectory
    {
        private readonly IGameAssemblyLoader _gameAssemblyLoader;
        private readonly Assembly _assembly;
        private readonly IDirectory _gameData;

        private IDirectory _location;

        public GetAssemblyDirectory([NotNull] IGameAssemblyLoader gameAssemblyLoader, Assembly assembly, IDirectory gameData)
        {
            if (gameAssemblyLoader == null) throw new ArgumentNullException("gameAssemblyLoader");
            if (assembly == null) throw new ArgumentNullException("assembly");
            if (gameData == null) throw new ArgumentNullException("gameData");
            if (string.IsNullOrEmpty(assembly.CodeBase))
                throw new InvalidOperationException("Assembly must have been loaded from disk");

            _gameAssemblyLoader = gameAssemblyLoader;
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

            var loaded = _gameAssemblyLoader.LoadedAssemblies.FirstOrDefault(la => ReferenceEquals(la.assembly, _assembly));
            if (loaded.IsNull()) return;

            var possible = _gameData.Directory(new KSPUrlIdentifier(loaded.url));

            if (possible.Any()) _location = possible.Single();
        }
    }
}
