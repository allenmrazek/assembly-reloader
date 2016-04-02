using System;
using System.Reflection;
using strange.extensions.injector.api;

namespace AssemblyReloader.Config
{
    [Implements(typeof(ILoadedAssemblyUninstaller), InjectionBindingScope.CROSS_CONTEXT)]
// ReSharper disable once ClassNeverInstantiated.Global
    public class LoadedAssemblyUninstaller : ILoadedAssemblyUninstaller
    {
        public bool Uninstall(Assembly target)
        {
            if (target == null) throw new ArgumentNullException("target");

            for (var idx = 0; idx < AssemblyLoader.loadedAssemblies.Count; ++idx)
                if (ReferenceEquals(AssemblyLoader.loadedAssemblies[idx].assembly, target))
                {
                    AssemblyLoader.loadedAssemblies.RemoveAt(idx);
                    return true;
                }

            return false;
        }
    }
}
