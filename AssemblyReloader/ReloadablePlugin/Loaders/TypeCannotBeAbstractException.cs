using System;

namespace AssemblyReloader.ReloadablePlugin.Loaders
{
    public class TypeCannotBeAbstractException : Exception
    {
        public TypeCannotBeAbstractException() : base("Type cannot be abstract and must be constructable")
        {
            
        }

        public TypeCannotBeAbstractException(string message) : base(message)
        {
        }

        public TypeCannotBeAbstractException(string message, Exception innerException) : base(message, innerException)
        {
            
        }

        public TypeCannotBeAbstractException(Type type) : base(type.FullName + " must be constructible")
        {
            
        }
    }
}
