using System;
using System.Linq;
using System.Reflection;
using AssemblyReloader.Annotations;
using AssemblyReloader.Loaders;
using AssemblyReloader.Providers;
using AssemblyReloader.Weaving;
using Mono.Cecil;
using ReeperCommon.Containers;
using ReeperCommon.Extensions;

namespace AssemblyReloader.Game
{
    public class KspAssemblyLoader : IAssemblyLoader
    {
        private readonly IAssemblyProvider _assemblyProvider;
        private readonly ILoadedAssemblyFactory _laFactory;
        private IDisposable _loadedAssembly;

        public KspAssemblyLoader(
            [NotNull] IAssemblyProvider assemblyProvider, 
            [NotNull] ILoadedAssemblyFactory laFactory)
        {
            if (assemblyProvider == null) throw new ArgumentNullException("assemblyProvider");
            if (laFactory == null) throw new ArgumentNullException("laFactory");

            _assemblyProvider = assemblyProvider;
            _laFactory = laFactory;
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
            if (!_loadedAssembly.IsNull())
                _loadedAssembly.Dispose();

            _loadedAssembly = null;
        }
    }
}
