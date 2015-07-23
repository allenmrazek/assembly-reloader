using System;
using ReeperCommon.FileSystem;

namespace AssemblyReloader.ReloadablePlugin.Weaving.Operations
{
    public class AssemblyCodeBase
    {
        public string Value { get; private set; }

        public AssemblyCodeBase(IFile location)
        {
            if (location == null) throw new ArgumentNullException("location");

            Value = "Todo: set assembly CodeBase from file location";
        }
    }
}
