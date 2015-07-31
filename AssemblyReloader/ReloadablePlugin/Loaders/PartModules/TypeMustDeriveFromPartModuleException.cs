using System;

namespace AssemblyReloader.ReloadablePlugin.Loaders.PartModules
{
    public class TypeMustDeriveFromPartModuleException : Exception
    {
        public TypeMustDeriveFromPartModuleException(Type type)
            : base(type.FullName + " must be a subclass of PartModule")
        {
            
        }
    }
}
