using System;
using System.Linq;
using System.Reflection;
using AssemblyReloader.Annotations;
using AssemblyReloader.Game;
using ReeperCommon.Containers;
using ReeperCommon.FileSystem;
using ReeperCommon.FileSystem.Factories;
using ReeperCommon.Logging;

namespace AssemblyReloader.Queries.FileSystemQueries
{
    public class AssemblyFileLocationQuery : IAssemblyFileLocationQuery
    {
        private readonly IGameAssemblyLoader _gameAssemblyLoader;
        private readonly IFileSystemFactory _fsFactory;


        public AssemblyFileLocationQuery([NotNull] IGameAssemblyLoader gameAssemblyLoader,
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
            var url = results.First().url + "/" + results.First().dllName;
 
            return _fsFactory.GetGameDataDirectory().File(new KSPUrlIdentifier(url));
        }
    }
}
