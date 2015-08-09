using System;

namespace AssemblyReloader.ReloadablePlugin.Loaders
{
    public class TypeMustBeAMonoBehaviourException : Exception
    {
        public TypeMustBeAMonoBehaviourException() : base("Type must be a subclass of MonoBehaviour")
        {
            
        }

        public TypeMustBeAMonoBehaviourException(string message) : base(message)
        {
            
        }

        public TypeMustBeAMonoBehaviourException(string message, Exception innerException)
            : base(message, innerException)
        {
            
        }


        public TypeMustBeAMonoBehaviourException(Type type)
            : base(string.Format("{0} must inherit from MonoBehaviour (or a subclass)", type.FullName))
        {
            
        }
    }
}
