using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ReeperCommon.FileSystem;

namespace AssemblyReloader.AssemblyTracking.Implementations
{
    public struct ReloadableReloadableIdentity : IReloadableIdentity
    {
        private readonly IFile _location;

        public ReloadableReloadableIdentity(IFile location)
        {
            if (location == null) throw new ArgumentNullException("location");
            _location = location;
        }

        public string Name
        {
            get { return _location.Name; }
        }

        public string Location { get { return _location.FullPath; }}
    }
}