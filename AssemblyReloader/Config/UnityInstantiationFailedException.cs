using System;

namespace AssemblyReloader.Config
{
    public class UnityInstantiationFailedException : Exception
    {
        public UnityInstantiationFailedException() : base("Unity instantiation failed")
        {
            
        }


        public UnityInstantiationFailedException(string message) : base(message)
        {
            
        }


        public UnityInstantiationFailedException(string message, Exception innerException)
            : base(message, innerException)
        {
            
        }
    }
}
