using System;
using ReeperCommon.FileSystem;

namespace AssemblyReloader.ReloadablePlugin.Weaving.Operations
{
// ReSharper disable once ClassNeverInstantiated.Global
    public class AssemblyCodeBase
    {
        public string Value { get; private set; }

        public AssemblyCodeBase(IFile location)
        {
            if (location == null) throw new ArgumentNullException("location");

            var uri = new Uri(location.FullPath);

            Value = Uri.UnescapeDataString(uri.AbsoluteUri);
        }
    }
}
