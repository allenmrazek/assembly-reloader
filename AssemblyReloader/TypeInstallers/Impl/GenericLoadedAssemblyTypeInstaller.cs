using System;
using AssemblyReloader.Annotations;
using AssemblyReloader.ReloadablePlugin.Loaders;
using UnityEngine;

namespace AssemblyReloader.TypeInstallers.Impl
{
    public class GenericLoadedAssemblyTypeInstaller<T> : ILoadedAssemblyTypeInstaller where T : MonoBehaviour
    {
        private readonly IGetTypesFromAssembly _getType;

        public GenericLoadedAssemblyTypeInstaller([NotNull] IGetTypesFromAssembly getType)
        {
            if (getType == null) throw new ArgumentNullException("getType");
            _getType = getType;
        }


        public void Install([NotNull] AssemblyLoader.LoadedAssembly assembly)
        {
            if (assembly == null) throw new ArgumentNullException("assembly");
            if (assembly.assembly == null) throw new ArgumentException("LoadedAssembly.assembly is null");


            foreach (var type in _getType.Get(assembly.assembly))
                assembly.types.Add(typeof (T), type);

        }
    }
}
