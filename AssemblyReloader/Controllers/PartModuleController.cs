using System;
using System.Collections.Generic;
using System.Reflection;
using AssemblyReloader.CompositeRoot.Commands;
using AssemblyReloader.Loaders;
using AssemblyReloader.Queries.AssemblyQueries;
using AssemblyReloader.Repositories;
using ReeperCommon.FileSystem;
using ReeperCommon.Logging;

namespace AssemblyReloader.Controllers
{
    public class PartModuleController : IReloadableObjectController
    {
        private readonly IPartModuleLoader _pmLoader;
        private readonly IPartModuleUnloader _pmUnloader;
        private readonly ITypesFromAssemblyQuery _partModuleFromAssemblyQuery;
        private readonly IFlightConfigRepository _partModuleConfigRepository;
        private readonly ICommand _refreshPartActionWindows;
        private readonly ILog _log;

        public PartModuleController(
            IPartModuleLoader pmLoader,
            IPartModuleUnloader pmUnloader,
            ITypesFromAssemblyQuery partModuleFromAssemblyQuery,
            IFlightConfigRepository partModuleConfigRepository,
            ICommand refreshPartActionWindows,
            ILog log)
        {
            if (pmLoader == null) throw new ArgumentNullException("pmLoader");
            if (pmUnloader == null) throw new ArgumentNullException("pmUnloader");
            if (partModuleFromAssemblyQuery == null) throw new ArgumentNullException("partModuleFromAssemblyQuery");
            if (partModuleConfigRepository == null) throw new ArgumentNullException("partModuleConfigRepository");
            if (refreshPartActionWindows == null) throw new ArgumentNullException("refreshPartActionWindows");
            if (log == null) throw new ArgumentNullException("log");

            _pmLoader = pmLoader;
            _pmUnloader = pmUnloader;
            _partModuleFromAssemblyQuery = partModuleFromAssemblyQuery;
            _partModuleConfigRepository = partModuleConfigRepository;
            _refreshPartActionWindows = refreshPartActionWindows;
            _log = log;
        }


        public void Load(Assembly assembly, IFile location)
        {
            if (assembly == null) throw new ArgumentNullException("assembly");

            _log.Verbose("Loading PartModules from " + assembly.FullName);

            foreach (var t in GetPartModules(assembly))
            {
                _log.Debug("Loading PartModule " + t.FullName);

                _pmLoader.Load(t);
            }
            _partModuleConfigRepository.Clear();
            _refreshPartActionWindows.Execute();
        }


        public void Unload(Assembly assembly, IFile location)
        {
            if (assembly == null) throw new ArgumentNullException("assembly");

            _log.Verbose("Unloading PartModules from " + assembly.FullName);

            foreach (var t in GetPartModules(assembly))
            {
                _log.Debug("Unloading PartModule " + t.FullName);
                _pmUnloader.Unload(t);
            }

            _refreshPartActionWindows.Execute();
        }


        private IEnumerable<Type> GetPartModules(Assembly assembly)
        {
            return _partModuleFromAssemblyQuery.Get(assembly);
        }
    }
}
