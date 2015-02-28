using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using AssemblyReloader.Game;
using Mono.Cecil;
using ReeperCommon.Containers;
using ReeperCommon.FileSystem;

namespace AssemblyReloader.Loaders
{
    // The difference between this loader and the AssemblyDefinitionLoader is that KSP's loading
    // system will be aware of these assemblies (and so will be able to instantiate types from them).
    // This has a severe limitation though: since KSP's loader doesn't differentiate between types with
    // the same name in different assemblies, anything loaded by this is effectively a single-shot deal
    // with no possibility of reloading
    public class AssemblyDefinitionIntoKspLoader : AssemblyDefinitionLoaderBase, IAssemblyDefinitionLoader
    {
        private readonly IFile _location;
        private readonly IAssemblyLoader _kspLoader;

        public AssemblyDefinitionIntoKspLoader(IFile location, IAssemblyLoader kspLoader)
        {
            if (location == null) throw new ArgumentNullException("location");
            if (kspLoader == null) throw new ArgumentNullException("kspLoader");
            _location = location;
            _kspLoader = kspLoader;
        }
  

        public Maybe<Assembly> Load(AssemblyDefinition definition)
        {
            using (var ms = WriteDefinitionToStream(definition))
            {
                return _kspLoader.Load(ms, _location);
            }
        }
    }
}
