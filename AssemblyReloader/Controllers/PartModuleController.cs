using System;
using System.Collections.Generic;
using System.Reflection;
using AssemblyReloader.Annotations;
using AssemblyReloader.Commands;
using AssemblyReloader.CompositeRoot;
using AssemblyReloader.DataObjects;
using AssemblyReloader.Loaders.PartModuleLoader;
using AssemblyReloader.Queries.AssemblyQueries;
using ReeperCommon.FileSystem;

namespace AssemblyReloader.Controllers
{
    public class PartModuleController : IReloadableObjectController
    {
        private readonly IPartModuleLoader _pmLoader;
        private readonly IPartModuleUnloader _pmUnloader;
        private readonly ITypesFromAssemblyQuery _partModuleFromAssemblyQuery;
        private readonly DictionaryQueue<KeyValuePair<uint, ITypeIdentifier>, ConfigNode> _partModuleConfigQueue;
        private readonly ICommand _refreshPartActionWindows;

        public PartModuleController(
            IPartModuleLoader pmLoader,
            IPartModuleUnloader pmUnloader,
            ITypesFromAssemblyQuery partModuleFromAssemblyQuery,
            [NotNull] DictionaryQueue<KeyValuePair<uint, ITypeIdentifier>, ConfigNode> partModuleConfigQueue,
            ICommand refreshPartActionWindows)
        {
            if (pmLoader == null) throw new ArgumentNullException("pmLoader");
            if (pmUnloader == null) throw new ArgumentNullException("pmUnloader");
            if (partModuleFromAssemblyQuery == null) throw new ArgumentNullException("partModuleFromAssemblyQuery");
            if (partModuleConfigQueue == null) throw new ArgumentNullException("partModuleConfigQueue");
            if (refreshPartActionWindows == null) throw new ArgumentNullException("refreshPartActionWindows");

            _pmLoader = pmLoader;
            _pmUnloader = pmUnloader;
            _partModuleFromAssemblyQuery = partModuleFromAssemblyQuery;
            _partModuleConfigQueue = partModuleConfigQueue;
            _refreshPartActionWindows = refreshPartActionWindows;
        }


        public void Load(Assembly assembly, IFile location)
        {
            if (assembly == null) throw new ArgumentNullException("assembly");

            foreach (var t in GetPartModules(assembly))
                _pmLoader.Load(t);

            _partModuleConfigQueue.Clear();
            _refreshPartActionWindows.Execute();
        }


        public void Unload(Assembly assembly, IFile location)
        {
            if (assembly == null) throw new ArgumentNullException("assembly");


            foreach (var t in GetPartModules(assembly))
                _pmUnloader.Unload(t);
            
            _refreshPartActionWindows.Execute();
        }


        private IEnumerable<Type> GetPartModules(Assembly assembly)
        {
            return _partModuleFromAssemblyQuery.Get(assembly);
        }
    }
}
