using System;
using System.Collections.Generic;
using System.Reflection;
using AssemblyReloader.Game.Commands;
using AssemblyReloader.Queries.AssemblyQueries;
using Contracts;
using ReeperCommon.FileSystem;

namespace AssemblyReloader.Game
{
    public class KspLoadedAssemblyFactory : ILoadedAssemblyFactory
    {
        private readonly ITypesFromAssemblyQuery _partQuery;
        private readonly ITypesFromAssemblyQuery _partModuleQuery;
        private readonly ITypesFromAssemblyQuery _internalModuleQuery;
        private readonly ITypesFromAssemblyQuery _scenarioModuleQuery;
        private readonly ITypesFromAssemblyQuery _contractQuery;

        public KspLoadedAssemblyFactory(
            ITypesFromAssemblyQuery partQuery,
            ITypesFromAssemblyQuery partModuleQuery,
            ITypesFromAssemblyQuery internalModuleQuery,
            ITypesFromAssemblyQuery scenarioModuleQuery,
            ITypesFromAssemblyQuery contractQuery)
        {
            if (partQuery == null) throw new ArgumentNullException("partQuery");
            if (partModuleQuery == null) throw new ArgumentNullException("partModuleQuery");
            if (internalModuleQuery == null) throw new ArgumentNullException("internalModuleQuery");
            if (scenarioModuleQuery == null) throw new ArgumentNullException("scenarioModuleQuery");
            if (contractQuery == null) throw new ArgumentNullException("contractQuery");

            _partQuery = partQuery;
            _partModuleQuery = partModuleQuery;
            _internalModuleQuery = internalModuleQuery;
            _scenarioModuleQuery = scenarioModuleQuery;
            _contractQuery = contractQuery;
        }


        public IDisposable Create(Assembly assembly, IFile location)
        {
            var la = new AssemblyLoader.LoadedAssembly(assembly, location.FullPath, location.Url, null);

            AddTypes(la, typeof (PartModule), _partModuleQuery.Get(assembly));
            AddTypes(la, typeof (Part), _partQuery.Get(assembly));
            AddTypes(la, typeof (InternalModule), _internalModuleQuery.Get(assembly));
            AddTypes(la, typeof (ScenarioModule), _scenarioModuleQuery.Get(assembly));
            AddTypes(la, typeof (Contract), _contractQuery.Get(assembly));

            AssemblyLoader.loadedAssemblies.Add(la);

            return new DisposeLoadedAssembly(la);
        }


        private void AddTypes(AssemblyLoader.LoadedAssembly loaded, Type key, IEnumerable<Type> types)
        {
            foreach (var ty in types)
                loaded.types.Add(key, ty);
        }
    }
}
