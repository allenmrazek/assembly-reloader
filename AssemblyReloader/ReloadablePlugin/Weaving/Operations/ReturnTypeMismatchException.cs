using System;

namespace AssemblyReloader.ReloadablePlugin.Weaving.Operations
{
    class ReturnTypeMismatchException : Exception
    {
        public ReturnTypeMismatchException(Type expected, Type actual)
            : base("Return type of " + actual.FullName + " does not match expected " + expected.FullName)
        {
            
        }
    }
}
