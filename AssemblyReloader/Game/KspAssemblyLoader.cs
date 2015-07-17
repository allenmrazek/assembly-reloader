using System;
using System.Linq;
using System.Reflection;
using AssemblyReloader.Properties;
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


        public KspAssemblyLoader(
            [NotNull] IGetLoadedAssemblyFileUrl laFileUrl)
        {
            if (laFileUrl == null) throw new ArgumentNullException("laFileUrl");

            _laFileUrl = laFileUrl;
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

            //InstallTypes(la, typeof (PartModule), _partModuleQuery.Get(assembly));
            //InstallTypes(la, typeof (Part), _partQuery.Get(assembly));
            //InstallTypes(la, typeof (InternalModule), _internalModuleQuery.Get(assembly));
            //InstallTypes(la, typeof (ScenarioModule), _scenarioModuleQuery.Get(assembly));
            //InstallTypes(la, typeof (Contract), _contractQuery.Get(assembly));
            // todo: kerbal experience traits?
            // todo: kerbal experience effects?
            // todo: contracts
            // todo: strategies
            // todo: ScienceExperiments
            // todo: parts
            // todo: VesselModules?
            // todo: InternalModules?

            //foreach (var installer in _typeInstallers)
            //    installer.Install(la);

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
    }
}
