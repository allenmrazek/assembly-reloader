using System;
using System.IO;
using System.Linq;
using System.Reflection;
using AssemblyReloader.Game;
using AssemblyReloader.Properties;
using ReeperCommon.Containers;
using ReeperCommon.FileSystem;
using ReeperCommon.Logging;

namespace AssemblyReloader.FileSystem
{
    [Implements(typeof(IGetAssemblyFileLocation))]
// ReSharper disable once UnusedMember.Global
    public class GetAssemblyFileLocation : IGetAssemblyFileLocation
    {
        private readonly IGameAssemblyLoader _gameAssemblyLoader;
        private readonly IFileSystemFactory _fsFactory;


        public GetAssemblyFileLocation(
            IGameAssemblyLoader gameAssemblyLoader,
            IFileSystemFactory fsFactory)
        {
            if (gameAssemblyLoader == null) throw new ArgumentNullException("gameAssemblyLoader");
            if (fsFactory == null) throw new ArgumentNullException("fsFactory");

            _gameAssemblyLoader = gameAssemblyLoader;
            _fsFactory = fsFactory;
        }


        public Maybe<IFile> Get([NotNull] Assembly target)
        {
            if (target == null) throw new ArgumentNullException("target");

            var log = new DebugLog("Temp");

            var results = _gameAssemblyLoader.LoadedAssemblies.Where(la => ReferenceEquals(la.assembly, target)).ToList();

            _gameAssemblyLoader.LoadedAssemblies.ToList()
                .ForEach(k => log.Normal("LoadedAssembly URL of " + k.path + ": " + k.url));

            if (results.Count > 1)
                throw new InvalidOperationException("Multiple targets found in assembly loader");

            if (!results.Any())
            {
                log.Error("Did not find " + target.GetName().Name + " in loaded assemblies");
                return Maybe<IFile>.None;
            }

            // oddly, the urls in AssemblyLoader don't specify the filename, only the directory
            var url = new KSPUrlIdentifier(results.First().url + Path.DirectorySeparatorChar + results.First().dllName);

            log.Normal("Url: " + url.Url);

            _gameAssemblyLoader.LoadedAssemblies.ToList().ForEach(lam => log.Normal("LoadedAssembly: " + lam.path));
            _gameAssemblyLoader.LoadedAssemblies.Select(la => la.url).ToList().ForEach(a => log.Normal("Url: " + a));

            var file = _fsFactory.GameData.File(url);
            if (!file.Any())
            {
                log.Error("didn't find the file");
            }
            else
            {
                log.Normal("found file");
                log.Normal("fullpath: " + file.Single().FullPath);
            }

            return _fsFactory.GameData.File(url);
        }
    }
}
