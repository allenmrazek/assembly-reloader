using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AssemblyReloader.StrangeIoC.extensions.implicitBind;
using AssemblyReloader.StrangeIoC.extensions.injector.api;
using ReeperCommon.Containers;
using ReeperCommon.Extensions;
using ReeperCommon.FileSystem;
using ReeperCommon.Logging;

namespace AssemblyReloader.Game
{
    [Implements(typeof(IGameAssemblyLoader), InjectionBindingScope.CROSS_CONTEXT)]
// ReSharper disable once ClassNeverInstantiated.Global
// ReSharper disable once UnusedMember.Global
    public class KspAssemblyLoader : IGameAssemblyLoader
    {
        private readonly IGetLoadedAssemblyFileUrl _laFileUrl;
        private readonly IGetTypesDerivedFrom<PartModule> _partModules;
        private readonly IGetTypesDerivedFrom<ScenarioModule> _scenarioModules;
        private readonly IGetTypesDerivedFrom<VesselModule> _vesselModules;


        public KspAssemblyLoader(
            IGetLoadedAssemblyFileUrl laFileUrl,
            IGetTypesDerivedFrom<PartModule> partModules,
            IGetTypesDerivedFrom<ScenarioModule> scenarioModules,
            IGetTypesDerivedFrom<VesselModule> vesselModules)
        {
            if (laFileUrl == null) throw new ArgumentNullException("laFileUrl");
            if (partModules == null) throw new ArgumentNullException("partModules");
            if (scenarioModules == null) throw new ArgumentNullException("scenarioModules");
            if (vesselModules == null) throw new ArgumentNullException("vesselModules");

            _laFileUrl = laFileUrl;
            _partModules = partModules;
            _scenarioModules = scenarioModules;
            _vesselModules = vesselModules;
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
                throw new DuplicateLoadedAssemblyException(assembly, location);

            var loadedAssembly = new AssemblyLoader.LoadedAssembly(assembly, location.FullPath, _laFileUrl.Get(location), null);

            AssemblyLoader.loadedAssemblies.Add(loadedAssembly);

            InstallTypes(loadedAssembly);

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

            throw new LoadedAssemblyHandleNotFoundException(handle);
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


        // The game looks inside this type list for certain types; we need to make sure they're there to be found
        private void InstallTypes(AssemblyLoader.LoadedAssembly loadedAssembly)
        {
            var partModules = _partModules.Get(loadedAssembly.assembly).ToList();
            var scenarioModules = _scenarioModules.Get(loadedAssembly.assembly).ToList();
            var vesselModules = _vesselModules.Get(loadedAssembly.assembly).ToList();

            InsertTypeListIntoLoadedAssembly<PartModule>(loadedAssembly, partModules);
            InsertTypeListIntoLoadedAssembly<ScenarioModule>(loadedAssembly, scenarioModules);
            InsertTypeListIntoLoadedAssembly<VesselModule>(loadedAssembly, vesselModules);
        }


        private static void InsertTypeListIntoLoadedAssembly<TKey>(
            AssemblyLoader.LoadedAssembly loadedAssembly, 
            List<Type> typesToInsert)
        {
            if (loadedAssembly == null) throw new ArgumentNullException("loadedAssembly");
            if (typesToInsert == null) throw new ArgumentNullException("typesToInsert");

            List<Type> loadedTypes;

            if (!loadedAssembly.types.TryGetValue(typeof (TKey), out loadedTypes))
                loadedAssembly.types.Add(typeof (TKey), typesToInsert);
            else loadedTypes.AddRange(typesToInsert);
        }
    }
}
