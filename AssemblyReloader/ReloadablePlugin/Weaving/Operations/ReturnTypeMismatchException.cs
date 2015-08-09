using System;

namespace AssemblyReloader.ReloadablePlugin.Weaving.Operations
{
    class ReturnTypeMismatchException : Exception
    {
        public ReturnTypeMismatchException() : base("Return type mismatch")
        {
            
        }

        public ReturnTypeMismatchException(string message) : base(message)
        {
        }

        public ReturnTypeMismatchException(string message, Exception innerException) : base(message, innerException)
        {
            
        }

        public ReturnTypeMismatchException(Type expected, Type actual)
            : base("Return type of " + actual.FullName + " does not match expected " + expected.FullName)
        {
            
        }
    }
}
