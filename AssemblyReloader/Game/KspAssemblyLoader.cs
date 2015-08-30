extern alias KSP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ReeperAssemblyLibrary;
using ReeperAssemblyLibrary.Factories;
using ReeperCommon.Containers;
using ReeperCommon.Extensions;
using ReeperCommon.FileSystem;
using ReeperCommon.Logging;
using strange.extensions.injector.api;

namespace AssemblyReloader.Game
{
    [Implements(typeof(IGameAssemblyLoader), InjectionBindingScope.CROSS_CONTEXT)]
// ReSharper disable once ClassNeverInstantiated.Global
// ReSharper disable once UnusedMember.Global
    public class KspAssemblyLoader : IGameAssemblyLoader
    {
        private readonly ILoadedAssemblyInstaller _installer;
        private readonly IReeperAssemblyFactory _factory;


        public KspAssemblyLoader(
            ILoadedAssemblyInstaller installer,
            IReeperAssemblyFactory factory)
        {
            if (installer == null) throw new ArgumentNullException("installer");
            if (factory == null) throw new ArgumentNullException("factory");

            _installer = installer;
            _factory = factory;
        }


        public ILoadedAssemblyHandle AddToLoadedAssemblies(Assembly assembly, IFile location)
        {
            if (assembly == null) throw new ArgumentNullException("assembly");
            if (location == null) throw new ArgumentNullException("location");
            if (KSP::AssemblyLoader.loadedAssemblies == null)
                throw new InvalidOperationException("AssemblyLoader.loadedAssemblies is null");

            return new LoadedAssemblyHandle(_installer.Install(assembly, _factory.Create(location.UrlFile.file)));

            //new DebugLog().Normal("LoadedAssembly URL of " + location.FileName + " is " + _laFileUrl.Get(location));

            //var url = _laFileUrl.Get(location);

            //if (KSP::AssemblyLoader.loadedAssemblies.Any(la => la.url == url && la.dllName == location.Name))
            //    throw new DuplicateLoadedAssemblyException(assembly, location);

            //var loadedAssembly = new KSP::AssemblyLoader.LoadedAssembly(assembly, location.FullPath, _laFileUrl.Get(location), null);

            //KSP::AssemblyLoader.loadedAssemblies.Add(loadedAssembly);

            //InstallTypes(loadedAssembly);

            //return Maybe<ILoadedAssemblyHandle>.With(new LoadedAssemblyHandle(loadedAssembly));
        }


        public void RemoveFromLoadedAssemblies(ILoadedAssemblyHandle handle)
        {
            if (handle == null) throw new ArgumentNullException("handle");

            for (var idx = 0; idx < KSP::AssemblyLoader.loadedAssemblies.Count; ++idx)
                if (ReferenceEquals(handle.LoadedAssembly, KSP::AssemblyLoader.loadedAssemblies[idx]))
                {
                    KSP::AssemblyLoader.loadedAssemblies.RemoveAt(idx);
                    return;
                }

            throw new LoadedAssemblyHandleNotFoundException(handle);
        }


        public KSP::AssemblyLoader.LoadedAssembyList LoadedAssemblies
        {
            get { return KSP::AssemblyLoader.loadedAssemblies; }
            set
            {
                if (value.IsNull()) throw new ArgumentNullException("value");

                KSP::AssemblyLoader.loadedAssemblies = value;
            }
        }
    }
}
