using System;

namespace AssemblyReloader.ReloadablePlugin.Weaving.Operations.GameEventInterception
{
    public class GetGenericParameterType : IGetGenericParameterType
    {
        public Type Get(Type genericType, int parameterIndex)
        {
            if (genericType == null)
                throw new ArgumentNullException("genericType");

            if (!genericType.IsGenericType || genericType.GetGenericArguments().Length == 0)
                throw new ArgumentException(genericType.FullName + " is not an instance of a generic type", "genericType");

            if (parameterIndex >= genericType.GetGenericArguments().Length || parameterIndex < 0)
                throw new ArgumentException("Index of generic parameter out of range", "parameterIndex");

            return genericType.GetGenericArguments()[parameterIndex];
        }
    }
}
