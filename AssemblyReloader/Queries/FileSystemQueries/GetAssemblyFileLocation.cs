using System;
using System.IO;
using System.Linq;
using System.Reflection;
using AssemblyReloader.Game;
using AssemblyReloader.Properties;
using ReeperCommon.Containers;
using ReeperCommon.FileSystem;

namespace AssemblyReloader.Queries.FileSystemQueries
{
    public class GetAssemblyFileLocation : IGetAssemblyFileLocation
    {
        private readonly IGameAssemblyLoader _gameAssemblyLoader;
        private readonly IFileSystemFactory _fsFactory;


        public GetAssemblyFileLocation([NotNull] IGameAssemblyLoader gameAssemblyLoader,
            [NotNull] IFileSystemFactory fsFactory)
        {
            if (gameAssemblyLoader == null) throw new ArgumentNullException("gameAssemblyLoader");
            if (fsFactory == null) throw new ArgumentNullException("fsFactory");

            _gameAssemblyLoader = gameAssemblyLoader;
            _fsFactory = fsFactory;
        }


        public Maybe<IFile> Get([NotNull] Assembly target)
        {
            if (target == null) throw new ArgumentNullException("target");

            var results = _gameAssemblyLoader.LoadedAssemblies.Where(la => ReferenceEquals(la.assembly, target)).ToList();

            if (results.Count > 1)
                throw new InvalidOperationException("Multiple targets found in assembly loader");

            if (!results.Any()) return Maybe<IFile>.None;

            // oddly, the urls in AssemblyLoader don't specify the filename, only the directory
            var url = results.First().url + Path.DirectorySeparatorChar + results.First().dllName;
 
            return _fsFactory.GameData.File(new KSPUrlIdentifier(url));
        }
    }
}
