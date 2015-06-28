using System;
using System.Collections.Generic;
using System.Reflection;
using AssemblyReloader.Properties;
using AssemblyReloader.Queries.FileSystemQueries;
using AssemblyReloader.TypeInstallers;
using ReeperCommon.Extensions;
using ReeperCommon.FileSystem;
using ReeperCommon.Logging;

namespace AssemblyReloader.Game
{
    [Implements(typeof(IGameAssemblyLoader))]
// ReSharper disable once ClassNeverInstantiated.Global
    public class KspAssemblyLoader : IGameAssemblyLoader
    {
        private readonly IEnumerable<ILoadedAssemblyTypeInstaller> _typeInstallers;
        private readonly IGetLoadedAssemblyFileUrl _laFileUrl;


        public KspAssemblyLoader(
            [NotNull] IEnumerable<ILoadedAssemblyTypeInstaller> typeInstallers,
            [NotNull] IGetLoadedAssemblyFileUrl laFileUrl)
        {
            if (typeInstallers == null) throw new ArgumentNullException("typeInstallers");
            if (laFileUrl == null) throw new ArgumentNullException("laFileUrl");

            _typeInstallers = typeInstallers;
            _laFileUrl = laFileUrl;
        }


        public ILoadedAssemblyHandle Load(Assembly assembly, IFile location)
        {
            if (assembly == null) throw new ArgumentNullException("assembly");
            if (location == null) throw new ArgumentNullException("location");
            if (AssemblyLoader.loadedAssemblies == null)
                throw new InvalidOperationException("AssemblyLoader.loadedAssemblies is null");

            new DebugLog().Normal("LoadedAssembly URL of " + location.FileName + " is " + _laFileUrl.Get(location));

            if (assembly == null) throw new ArgumentNullException("assembly");
            if (location == null) throw new ArgumentNullException("location");

            var la = new AssemblyLoader.LoadedAssembly(assembly, location.FullPath, _laFileUrl.Get(location), null);

            AssemblyLoader.loadedAssemblies.Add(la);

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

            foreach (var installer in _typeInstallers)
                installer.Install(la);

            return new LoadedAssemblyHandle(la);
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
