using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using AssemblyReloader.Destruction;
using AssemblyReloader.Loaders;
using AssemblyReloader.Loaders.PMLoader;
using AssemblyReloader.Providers.SceneProviders;
using AssemblyReloader.Queries.AssemblyQueries;
using AssemblyReloader.Repositories;
using ReeperCommon.FileSystem;
using ReeperCommon.Logging;

namespace AssemblyReloader.Controllers
{
    public class PartModuleController : IReloadableObjectController
    {
        private readonly IPersistentObjectLoader _pmLoader;
        private readonly IPartModuleUnloader _pmUnloader;
        private readonly ITypesFromAssemblyQuery _partModuleFromAssemblyQuery;
        private readonly IFlightConfigRepository _partModuleConfigRepository;
        private readonly ILog _log;

        public PartModuleController(
            IPersistentObjectLoader pmLoader,
            IPartModuleUnloader pmUnloader,
            ITypesFromAssemblyQuery partModuleFromAssemblyQuery,
            IFlightConfigRepository partModuleConfigRepository,
            ILog log)
        {
            if (pmLoader == null) throw new ArgumentNullException("pmLoader");
            if (pmUnloader == null) throw new ArgumentNullException("pmUnloader");
            if (partModuleFromAssemblyQuery == null) throw new ArgumentNullException("partModuleFromAssemblyQuery");
            if (partModuleConfigRepository == null) throw new ArgumentNullException("partModuleConfigRepository");
            if (log == null) throw new ArgumentNullException("log");

            _pmLoader = pmLoader;
            _pmUnloader = pmUnloader;
            _partModuleFromAssemblyQuery = partModuleFromAssemblyQuery;
            _partModuleConfigRepository = partModuleConfigRepository;
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
        }

        private IEnumerable<Type> GetPartModules(Assembly assembly)
        {
            return _partModuleFromAssemblyQuery.Get(assembly);
        }
    }
}
