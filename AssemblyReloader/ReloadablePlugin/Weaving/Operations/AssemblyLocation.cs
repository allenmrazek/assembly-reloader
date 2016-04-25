using System;
using ReeperKSP.FileSystem;

namespace AssemblyReloader.ReloadablePlugin.Weaving.Operations
{
// ReSharper disable once ClassNeverInstantiated.Global
    public class AssemblyLocation
    {
        public string Value { get; private set; }

        public AssemblyLocation(IFile assembly)
        {
            if (assembly == null) throw new ArgumentNullException("assembly");

            var uri = new Uri(assembly.FullPath);

            Value = uri.LocalPath;
        }
    }
}
