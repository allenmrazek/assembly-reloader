using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using ReeperCommon.Containers;
using ReeperCommon.Extensions;
using ReeperCommon.FileSystem;
using ReeperCommon.Logging;

namespace AssemblyReloader.Game
{
    public class KspAssemblyLoader : IAssemblyLoader
    {
        private AssemblyLoader.LoadedAssembly _loadedAssembly;

        public void Load(Assembly assembly, IFile location)
        {
            if (assembly == null) throw new ArgumentNullException("assembly");
            if (location == null) throw new ArgumentNullException("location");

            if (AssemblyLoader.loadedAssemblies.GetByAssembly(assembly) != null)
                throw new InvalidOperationException(assembly.FullName + " has already been loaded by AssemblyLoader!");

            _loadedAssembly = new AssemblyLoader.LoadedAssembly(assembly, location.FullPath, location.Url, null);

            AssemblyLoader.loadedAssemblies.Add(_loadedAssembly);
        }


        public void Unload(Assembly assembly, IFile location)
        {
            if (assembly == null) throw new ArgumentNullException("assembly");
            if (location == null) throw new ArgumentNullException("location");
            if (_loadedAssembly == null) throw new Exception("AssemblyLoader.LoadedAssembly is null");

            for (int idx = 0; idx < AssemblyLoader.loadedAssemblies.Count; ++idx)
                if (ReferenceEquals(_loadedAssembly, AssemblyLoader.loadedAssemblies[idx]))
                {
                    AssemblyLoader.loadedAssemblies.RemoveAt(idx);
                    return;
                }

            throw new Exception("Failed to find assembly " + assembly.FullName + " (location " + location.Url +
                                ") in loaded assembly list");
        }
    }
}
