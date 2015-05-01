using System;
using System.Reflection;
using AssemblyReloader.Annotations;
using AssemblyReloader.Queries.AssemblyQueries;
using AssemblyReloader.Queries.FileSystemQueries;
using AssemblyReloader.TypeInstallers;
using ReeperCommon.FileSystem;
using ReeperCommon.Logging;

namespace AssemblyReloader.Game
{
    public class KspLoadedAssemblyFactory : ILoadedAssemblyFactory
    {
        private readonly ILoadedAssemblyFileUrlQuery _laFileUrlQuery;
        private readonly IDisposeLoadedAssemblyCommandFactory _disposeFactory;
        private readonly ITypeInstaller[] _typeInstallers;

        public KspLoadedAssemblyFactory(
            [NotNull] ILoadedAssemblyFileUrlQuery laFileUrlQuery,
            [NotNull] IDisposeLoadedAssemblyCommandFactory disposeFactory,
            [NotNull] params ITypeInstaller[] typeInstallers)
        {
            if (laFileUrlQuery == null) throw new ArgumentNullException("laFileUrlQuery");
            if (disposeFactory == null) throw new ArgumentNullException("disposeFactory");
            if (typeInstallers == null) throw new ArgumentNullException("typeInstallers");

            _laFileUrlQuery = laFileUrlQuery;
            _disposeFactory = disposeFactory;
            _typeInstallers = typeInstallers;
        }


        public IDisposable Create([NotNull] Assembly assembly, [NotNull] IFile location)
        {
            if (assembly == null) throw new ArgumentNullException("assembly");
            if (location == null) throw new ArgumentNullException("location");
            if (AssemblyLoader.loadedAssemblies == null)
                throw new InvalidOperationException("AssemblyLoader.loadedAssemblies is null");

            new DebugLog().Normal("LoadedAssembly URL of " + location.FileName + " is " + _laFileUrlQuery.Get(location));

            var la = new AssemblyLoader.LoadedAssembly(assembly, location.FullPath, _laFileUrlQuery.Get(location), null);

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

            foreach (var installer in _typeInstallers)
                installer.Install(la);


            AssemblyLoader.loadedAssemblies.Add(la);


            return _disposeFactory.Create(la);
        }
    }
}
