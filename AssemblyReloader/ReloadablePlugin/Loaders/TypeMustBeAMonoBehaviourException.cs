using System;

namespace AssemblyReloader.ReloadablePlugin.Loaders
{
    public class TypeMustBeAMonoBehaviourException : Exception
    {
        public TypeMustBeAMonoBehaviourException(Type type)
            : base(string.Format("{0} must inherit from MonoBehaviour (or a derivative)", type.FullName))
        {
            
        }
    }
}
