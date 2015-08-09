using System;
using System.Reflection;

namespace AssemblyReloader
{
    public class AssemblyFileLocationNotFoundException : Exception
    {
        public AssemblyFileLocationNotFoundException() : base("Assembly file location not found")
        {
            
        }


        public AssemblyFileLocationNotFoundException(string message) : base(message)
        {
            
        }


        public AssemblyFileLocationNotFoundException(Assembly assembly)
            : base("Failed to find file location of " + assembly.GetName().FullName)
        {
            
        }


        public AssemblyFileLocationNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
            
        }
    }
}
