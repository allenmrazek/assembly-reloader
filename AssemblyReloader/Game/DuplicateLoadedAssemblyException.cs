using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ReeperCommon.FileSystem;

namespace AssemblyReloader.Game
{
    public class DuplicateLoadedAssemblyException : Exception
    {
        public DuplicateLoadedAssemblyException() : base("A duplicate assembly already exists")
        {
            
        }

        public DuplicateLoadedAssemblyException(Assembly assembly, IFile location)
            : base(
                "A duplicate assembly of " + assembly.GetName().FullName + " from " + location.Url + " already exists")
        {

        }


        public DuplicateLoadedAssemblyException(string message)
            : base(message)
        {
            
        }


        public DuplicateLoadedAssemblyException(string message, Exception innerException)
            : base(message, innerException)
        {
            
        }
    }
}
