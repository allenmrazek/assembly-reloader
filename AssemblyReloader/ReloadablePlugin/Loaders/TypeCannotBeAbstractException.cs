using System;

namespace AssemblyReloader.ReloadablePlugin.Loaders
{
    public class TypeCannotBeAbstractException : Exception
    {
        public TypeCannotBeAbstractException(Type type) : base(type.FullName + " must be constructible")
        {
            
        }
    }
}
