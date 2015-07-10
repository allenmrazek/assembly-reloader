//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection;
//using System.Runtime.Serialization;
//using System.Text;
//using AssemblyReloader.StrangeIoC.framework.impl;
//using ReeperCommon.Containers;
//using ReeperCommon.Serialization;

//namespace AssemblyReloader.Config
//{

//    public class SerializerSelectorWithGenericSupport : DefaultConfigNodeItemSerializerSelector
//    {
//        private readonly IEnumerable<Type> _genericSurrogates;

//        public SerializerSelectorWithGenericSupport(IEnumerable<Type> genericSurrogates)
//        {
//            if (genericSurrogates == null) throw new ArgumentNullException("genericSurrogates");

//            var surrogates = genericSurrogates.ToList();

//            if (!surrogates.All(t => typeof(ISurrogateSerializer).IsAssignableFrom(t)))
//                throw new ArgumentException("All surrogates must implement ISurrogateSerializer");

//            if (!surrogates.All(t => t.IsValueType || t.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, Type.EmptyTypes, null) != null))
//                throw new ArgumentException("All generic surrogate types must have default constructors");

//            _genericSurrogates = surrogates;
//        }


//        protected override Maybe<ISurrogateSerializer> GetGenericSurrogate(Type target)
//        {
//            // target is going to be SomeType<T>
//            //
//            // we're hunting for a generic type of the form ISerializationSurrogate<SomeType<T>>

//            var surrogateOptions =
//                _genericSurrogates.Where(surrogateType => SurrogateCanSerializeType(surrogateType, target)).ToList();

//            if (!surrogateOptions.Any())
//                return Maybe<ISurrogateSerializer>.None;
            
//            if (surrogateOptions.Count > 1)
//                throw new AmbiguousMatchException("Multiple generic serialization matches for " + target.FullName);

//            return
//                Maybe<ISurrogateSerializer>.With((ISurrogateSerializer)
//                    Activator.CreateInstance(surrogateOptions.First().MakeGenericType(target, null)));

//        }


//        public bool SurrogateCanSerializeType(Type potentialSurrogate, Type thatNeedsSerialization)
//        {
//            if (potentialSurrogate == null) throw new ArgumentNullException("potentialSurrogate");
//            if (thatNeedsSerialization == null) throw new ArgumentNullException("thatNeedsSerialization");

//            if (!thatNeedsSerialization.IsGenericType)
//                return false;

//            return potentialSurrogate.GetInterfaces()
//                .Where(i => i.IsGenericType)
//                .Where(i => i.GetGenericArguments().Length == 1)
//                .Where(i => i.GetGenericArguments().First().IsGenericType)
//                .Any(i => i.GetGenericArguments().First().GetGenericTypeDefinition() == thatNeedsSerialization.GetGenericTypeDefinition());
//        }
//    }
//}
