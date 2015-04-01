using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AssemblyReloader.Annotations;
using AssemblyReloader.Commands;
using AssemblyReloader.Queries.AssemblyQueries;
using AssemblyReloader.TypeInstallers;
using Contracts;
using ReeperCommon.FileSystem;
using ReeperCommon.Logging.Implementations;

namespace AssemblyReloader.Game
{
    public class KspLoadedAssemblyFactory : ILoadedAssemblyFactory
    {
        private readonly IDisposeLoadedAssemblyCommandFactory _disposeFactory;
        private readonly ITypeInstaller[] _typeInstallers;

        public KspLoadedAssemblyFactory(
            [NotNull] IDisposeLoadedAssemblyCommandFactory disposeFactory,
            [NotNull] params ITypeInstaller[] typeInstallers)
        {
            if (disposeFactory == null) throw new ArgumentNullException("disposeFactory");
            if (typeInstallers == null) throw new ArgumentNullException("typeInstallers");

            _disposeFactory = disposeFactory;
            _typeInstallers = typeInstallers;
        }


        public IDisposable Create(Assembly assembly, IFile location)
        {
            var la = new AssemblyLoader.LoadedAssembly(assembly, location.FullPath, location.Url, null);

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

            foreach (var installer in _typeInstallers)
                installer.Install(la);

            var log = new DebugLog("LoadedAssemblyFactory");

            log.Normal("Types in LoadedAssembly:");

            foreach (var typeName in la.types.Keys.OrderBy(k => k.Name))
                foreach (var t in la.types[typeName].OrderBy(t => t.Name))
                    log.Normal(typeName + ": " + t.FullName + "; " + t.AssemblyQualifiedName);

            log.Normal("End loaded type list");

            AssemblyLoader.loadedAssemblies.Add(la);

            return _disposeFactory.Create(la);
        }


        //private static void InstallTypes(AssemblyLoader.LoadedAssembly loaded, Type key, IEnumerable<Type> types)
        //{
        //    foreach (var ty in types)
        //        loaded.types.Add(key, ty);
        //}
    }
}
