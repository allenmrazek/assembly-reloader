using System;
using ReeperCommon.FileSystem;

namespace AssemblyReloader.ReloadablePlugin.Weaving.Operations
{
    public class AssemblyLocation
    {
        public string Value { get; private set; }

        public AssemblyLocation(IFile assembly)
        {
            if (assembly == null) throw new ArgumentNullException("assembly");

            // todo: get Assembly.Location using assembly file location
            Value = "Todo: set this value";
        }
    }
}
