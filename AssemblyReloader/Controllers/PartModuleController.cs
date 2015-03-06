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

namespace AssemblyReloader.Controllers
{
    public class PartModuleController : IPersistentObjectController<PartModule>
    {
        private readonly IPersistentObjectLoader _pmLoader;
        private readonly IPartModuleUnloader _pmUnloader;
        private readonly ITypesFromAssemblyQuery _partModuleFromAssemblyQuery;
        private readonly IFlightConfigRepository _partModuleConfigRepository;
        private readonly ICurrentSceneIsFlightQuery _flightQuery;

        public PartModuleController(
            IPersistentObjectLoader pmLoader,
            IPartModuleUnloader pmUnloader,
            ITypesFromAssemblyQuery partModuleFromAssemblyQuery,
            IFlightConfigRepository partModuleConfigRepository,
            ICurrentSceneIsFlightQuery flightQuery)
        {
            if (pmLoader == null) throw new ArgumentNullException("pmLoader");
            if (pmUnloader == null) throw new ArgumentNullException("pmUnloader");
            if (partModuleFromAssemblyQuery == null) throw new ArgumentNullException("partModuleFromAssemblyQuery");
            if (partModuleConfigRepository == null) throw new ArgumentNullException("partModuleConfigRepository");
            if (flightQuery == null) throw new ArgumentNullException("flightQuery");

            _pmLoader = pmLoader;
            _pmUnloader = pmUnloader;
            _partModuleFromAssemblyQuery = partModuleFromAssemblyQuery;
            _partModuleConfigRepository = partModuleConfigRepository;
            _flightQuery = flightQuery;
        }


        public void LoadPersistentObjects(Assembly assembly, IFile location)
        {
            if (assembly == null) throw new ArgumentNullException("assembly");

            foreach (var t in _partModuleFromAssemblyQuery.Get(assembly))
                _pmLoader.Load(t, _flightQuery.Get());

            _partModuleConfigRepository.Clear();
        }


        public void UnloadPersistentObjects(Assembly assembly, IFile location)
        {
            throw new NotImplementedException();
        }
    }
}
