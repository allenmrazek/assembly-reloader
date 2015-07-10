using System;
using System.Linq;
using ReeperCommon.Containers;
using ReeperCommon.Serialization;

namespace AssemblyReloader.Config
{
    // Normally the serialization system only selects concrete surrogateSerializer types when searching
    // for a way to serialize [Target]. If we allowed generic surrogates, there would be no way
    // to know which instances we'd need to create to handle them in advance
    //
    // This extension does exactly that: if we can't serialize target but we have a generic
    // surrogateSerializer that can, we'll create a surrogateSerializer for that type on request
    public class SettingSerializerSelector : IConfigNodeItemSerializerSelector
    {
        public readonly IConfigNodeItemSerializerSelector Selector;


        public SettingSerializerSelector(IConfigNodeItemSerializerSelector selector)
        {
            if (selector == null) throw new ArgumentNullException("selector");

            Selector = selector;
        }

        public Maybe<IConfigNodeItemSerializer> GetSerializer(Type target)
        {
            var result = Selector.GetSerializer(target);
            if (result.Any() || !target.IsGenericType || target.IsGenericTypeDefinition) return result;

            if (target.GetGenericTypeDefinition() != typeof(Setting<>) || target.GetGenericArguments().Length != 1)
                return result;

            // alright, what kind of Setting is it? We want to find out what T is in Setting<T>
            var wrappedType = target.GetGenericArguments().First();

            return Selector.GetSerializer(wrappedType);
        }
    }
}
