using System;
using System.Reflection;

namespace AssemblyReloader
{
    public class AssemblyFileLocationNotFoundException : Exception
    {
        public AssemblyFileLocationNotFoundException() : base("Failed to find assembly file location")
        {
            
        }


        public AssemblyFileLocationNotFoundException(Assembly assembly)
            : base("Failed to find file location of " + assembly.GetName().FullName)
        {
            
        }
    }
}
