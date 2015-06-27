using System;
using System.Collections.Generic;
using System.Reflection;
using AssemblyReloader.Annotations;
using AssemblyReloader.Commands;
using AssemblyReloader.Controllers;
using AssemblyReloader.Loaders.PartModuleLoader;
using AssemblyReloader.Queries.AssemblyQueries;
using ReeperCommon.FileSystem;

namespace AssemblyReloader.ReloadablePlugin
{
    public class PartModuleFacade : IReloadableObjectFacade
    {
        private readonly IPartModuleLoader _pmLoader;
        private readonly IPartModuleUnloader _pmUnloader;
        private readonly ITypesFromAssemblyQuery _partModuleFromAssemblyQuery;
        private readonly ICommand _onPartModulesLoaded;
        private readonly ICommand _onPartModulesUnloaded;

        public PartModuleFacade(
            [NotNull] IPartModuleLoader pmLoader,
            [NotNull] IPartModuleUnloader pmUnloader,
            [NotNull] ITypesFromAssemblyQuery partModuleFromAssemblyQuery,
            [NotNull] ICommand onPartModulesLoaded, 
            [NotNull] ICommand onPartModulesUnloaded)
        {
            if (pmLoader == null) throw new ArgumentNullException("pmLoader");
            if (pmUnloader == null) throw new ArgumentNullException("pmUnloader");
            if (partModuleFromAssemblyQuery == null) throw new ArgumentNullException("partModuleFromAssemblyQuery");
            if (onPartModulesLoaded == null) throw new ArgumentNullException("onPartModulesLoaded");
            if (onPartModulesUnloaded == null) throw new ArgumentNullException("onPartModulesUnloaded");

            _pmLoader = pmLoader;
            _pmUnloader = pmUnloader;
            _partModuleFromAssemblyQuery = partModuleFromAssemblyQuery;
            _onPartModulesLoaded = onPartModulesLoaded;
            _onPartModulesUnloaded = onPartModulesUnloaded;
        }


        public void Load(Assembly assembly, IFile location)
        {
            if (assembly == null) throw new ArgumentNullException("assembly");

            foreach (var t in GetPartModules(assembly))
                _pmLoader.Load(t);

            _onPartModulesLoaded.Execute();
        }


        public void Unload(Assembly assembly, IFile location)
        {
            if (assembly == null) throw new ArgumentNullException("assembly");

            foreach (var t in GetPartModules(assembly))
                _pmUnloader.Unload(t);
            
            _onPartModulesUnloaded.Execute();
        }


        private IEnumerable<Type> GetPartModules(Assembly assembly)
        {
            return _partModuleFromAssemblyQuery.Get(assembly);
        }
    }
}
