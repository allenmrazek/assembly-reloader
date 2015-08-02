using System;
using System.Linq;
using System.Reflection;
using AssemblyReloader.Properties;
using AssemblyReloader.ReloadablePlugin.Loaders;
using AssemblyReloader.StrangeIoC.extensions.implicitBind;
using ReeperCommon.Containers;
using ReeperCommon.Extensions;
using ReeperCommon.FileSystem;
using ReeperCommon.Logging;

namespace AssemblyReloader.Game
{
    [Implements(typeof(IGameAssemblyLoader))]
// ReSharper disable once ClassNeverInstantiated.Global
    public class KspAssemblyLoader : IGameAssemblyLoader
    {
        private readonly IGetLoadedAssemblyFileUrl _laFileUrl;
        private readonly IGetTypesDerivedFrom<PartModule> _partModuleTypeQuery;


        public KspAssemblyLoader(
            IGetLoadedAssemblyFileUrl laFileUrl,
            IGetTypesDerivedFrom<PartModule> partModuleTypeQuery)
        {
            if (laFileUrl == null) throw new ArgumentNullException("laFileUrl");
            if (partModuleTypeQuery == null) throw new ArgumentNullException("partModuleTypeQuery");

            _laFileUrl = laFileUrl;
            _partModuleTypeQuery = partModuleTypeQuery;
        }


        public Maybe<ILoadedAssemblyHandle> AddToLoadedAssemblies(Assembly assembly, IFile location)
        {
            if (assembly == null) throw new ArgumentNullException("assembly");
            if (location == null) throw new ArgumentNullException("location");
            if (AssemblyLoader.loadedAssemblies == null)
                throw new InvalidOperationException("AssemblyLoader.loadedAssemblies is null");

            new DebugLog().Normal("LoadedAssembly URL of " + location.FileName + " is " + _laFileUrl.Get(location));

            var url = _laFileUrl.Get(location);

            if (AssemblyLoader.loadedAssemblies.Any(la => la.url == url && la.dllName == location.Name))
                throw new Exception("An assembly with this URL and name is already loaded by KSP. Duplicate entries?");

            var loadedAssembly = new AssemblyLoader.LoadedAssembly(assembly, location.FullPath, _laFileUrl.Get(location), null);

            AssemblyLoader.loadedAssemblies.Add(loadedAssembly);

            InstallTypes(loadedAssembly);
            return Maybe<ILoadedAssemblyHandle>.With(new LoadedAssemblyHandle(loadedAssembly));
        }


        public void RemoveFromLoadedAssemblies(ILoadedAssemblyHandle handle)
        {
            if (handle == null) throw new ArgumentNullException("handle");

            for (var idx = 0; idx < AssemblyLoader.loadedAssemblies.Count; ++idx)
                if (ReferenceEquals(handle.LoadedAssembly, AssemblyLoader.loadedAssemblies[idx]))
                {
                    AssemblyLoader.loadedAssemblies.RemoveAt(idx);
                    return;
                }

            throw new Exception("handle was not found in AssemblyLoader.loadedAssemblies");
        }


        public AssemblyLoader.LoadedAssembyList LoadedAssemblies
        {
            get { return AssemblyLoader.loadedAssemblies; }
            set
            {
                if (value.IsNull()) throw new ArgumentNullException("value");

                AssemblyLoader.loadedAssemblies = value;
            }
        }


        // The game looks inside this type list for certain types; we need to make sure they're there to be found
        private void InstallTypes(AssemblyLoader.LoadedAssembly loadedAssembly)
        {
            var partModules = _partModuleTypeQuery.Get(loadedAssembly.assembly).ToList();

            loadedAssembly.types[typeof(PartModule)] = partModules;
        }
    }
}
