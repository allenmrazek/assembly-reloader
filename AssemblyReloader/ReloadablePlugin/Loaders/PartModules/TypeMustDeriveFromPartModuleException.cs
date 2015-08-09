using System;

namespace AssemblyReloader.ReloadablePlugin.Loaders.PartModules
{
    public class TypeMustDeriveFromPartModuleException : Exception
    {
        public TypeMustDeriveFromPartModuleException() : base("Type must be a subclass of PartModule")
        {
            
        }


        public TypeMustDeriveFromPartModuleException(string message)
            : base(message)
        {
            
        }


        public TypeMustDeriveFromPartModuleException(string message, Exception innerException)
            : base(message, innerException)
        {
            
        }


        public TypeMustDeriveFromPartModuleException(Type type)
            : base(type.FullName + " must be a subclass of PartModule")
        {
            
        }
    }
}
