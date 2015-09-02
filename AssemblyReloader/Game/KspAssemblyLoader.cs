extern alias KSP;
using System.Collections.Generic;
using System.Linq;
using ReeperAssemblyLibrary;
using strange.extensions.implicitBind;
using strange.extensions.injector.api;

namespace AssemblyReloader.Game
{
    [Implements(typeof(IGameAssemblyLoader), InjectionBindingScope.CROSS_CONTEXT)]
// ReSharper disable once ClassNeverInstantiated.Global
// ReSharper disable once UnusedMember.Global
    public class KspAssemblyLoader : IGameAssemblyLoader
    {
        public IEnumerable<ILoadedAssemblyHandle> LoadedAssemblies
        {
            get
            {
                return KSP::AssemblyLoader.loadedAssemblies
                    .Select<KSP::AssemblyLoader.LoadedAssembly, ILoadedAssemblyHandle>(la => new LoadedAssemblyHandle(la));
            }
        }
    }
}
