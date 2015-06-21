using System;
using AssemblyReloader.Annotations;
using AssemblyReloader.Queries.AssemblyQueries;
using UnityEngine;

namespace AssemblyReloader.TypeInstallers.Impl
{
    public class GenericTypeInstaller<T> : ITypeInstaller where T : MonoBehaviour
    {
        private readonly ITypesFromAssemblyQuery _typeQuery;

        public GenericTypeInstaller([NotNull] ITypesFromAssemblyQuery typeQuery)
        {
            if (typeQuery == null) throw new ArgumentNullException("typeQuery");
            _typeQuery = typeQuery;
        }


        public void Install([NotNull] AssemblyLoader.LoadedAssembly assembly)
        {
            if (assembly == null) throw new ArgumentNullException("assembly");
            if (assembly.assembly == null) throw new ArgumentException("LoadedAssembly.assembly is null");


            foreach (var type in _typeQuery.Get(assembly.assembly))
                assembly.types.Add(typeof (T), type);

        }
    }
}
