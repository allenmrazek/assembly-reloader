using System;
using System.Linq;
using System.Reflection;
using AssemblyReloader.Annotations;
using AssemblyReloader.Game;
using AssemblyReloader.Providers;
using ReeperCommon.Containers;
using ReeperCommon.Extensions;
using ReeperCommon.Logging;

namespace AssemblyReloader.Loaders
{
// ReSharper disable once ClassNeverInstantiated.Global
    public class AssemblyLoader : IAssemblyLoader
    {
        private readonly IAssemblyProvider _assemblyProvider;
        private readonly ILoadedAssemblyFactory _laFactory;
        private readonly ILog _log;
        private IDisposable _loadedAssembly;

        public AssemblyLoader(
            [NotNull] IAssemblyProvider assemblyProvider, 
            [NotNull] ILoadedAssemblyFactory laFactory,
            [NotNull] ILog log)
        {
            if (assemblyProvider == null) throw new ArgumentNullException("assemblyProvider");
            if (laFactory == null) throw new ArgumentNullException("laFactory");
            if (log == null) throw new ArgumentNullException("log");

            _assemblyProvider = assemblyProvider;
            _laFactory = laFactory;
            _log = log;
        }


        public Maybe<Assembly> Load()
        {
            if (!_loadedAssembly.IsNull())
                throw new InvalidOperationException(_assemblyProvider.Location.Name + " has not been unloaded");

            var result = _assemblyProvider.Get();

            if (!result.Any())
                throw new InvalidOperationException("Assembly provider did not provide any assembly");

            _loadedAssembly = _laFactory.Create(result.Single(), _assemblyProvider.Location);

            return result;
        }


        public void Unload()
        {
            try
            {
                if (!_loadedAssembly.IsNull())
                    _loadedAssembly.Dispose();
            }
            catch (Exception e)
            {
                _log.Warning("Failed to dispose of loaded assembly due to an uncaught exception: " + e);
            }
            finally
            {

                _loadedAssembly = null;
            }
        }
    }
}
