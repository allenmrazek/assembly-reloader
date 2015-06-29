//using System;
//using System.Linq;
//using System.Reflection;
//using AssemblyReloader.Annotations;
//using AssemblyReloader.Game;
//using AssemblyReloader.Providers;
//using ReeperCommon.Containers;
//using ReeperCommon.Extensions;
//using ReeperCommon.FileSystem;
//using ReeperCommon.Logging;

//namespace AssemblyReloader.Loaders
//{
//    // ReSharper disable once ClassNeverInstantiated.Global
//    public class AssemblyLoader : IAssemblyLoader
//    {
//        private readonly IFile _targetFile;
//        private readonly IAssemblyProvider _assemblyProvider;
//        private readonly ILoadedAssemblyFactory _laFactory;
//        private readonly ILog _log;
//        private IDisposable _loadedAssembly;

//        public AssemblyLoader(
//            [NotNull] IFile targetFile,
//            [NotNull] IAssemblyProvider assemblyProvider,
//            [NotNull] ILoadedAssemblyFactory laFactory,
//            [NotNull] ILog log)
//        {
//            if (targetFile == null) throw new ArgumentNullException("targetFile");
//            if (assemblyProvider == null) throw new ArgumentNullException("assemblyProvider");
//            if (laFactory == null) throw new ArgumentNullException("laFactory");
//            if (log == null) throw new ArgumentNullException("log");

//            _targetFile = targetFile;
//            _assemblyProvider = assemblyProvider;
//            _laFactory = laFactory;
//            _log = log;
//        }


//        public Maybe<Assembly> AddToLoadedAssemblies()
//        {
//            if (!_loadedAssembly.IsNull())
//                throw new InvalidOperationException(_targetFile.Url + " has not been unloaded");

//            var result = _assemblyProvider.Get(_targetFile);

//            if (!result.Any())
//                throw new InvalidOperationException("Assembly provider did not provide any assembly");

//            _loadedAssembly = _laFactory.Create(result.Single(), _targetFile);

//            return result;
//        }


//        public void DestroyReloadableTypesFrom()
//        {
//            try
//            {
//                if (!_loadedAssembly.IsNull())
//                    _loadedAssembly.Dispose();
//            }
//            catch (Exception e)
//            {
//                _log.Warning("Failed to dispose of loaded assembly due to an uncaught exception: " + e);
//            }
//            finally
//            {

//                _loadedAssembly = null;
//            }
//        }
//    }
//}
