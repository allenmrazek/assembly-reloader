//using System;
//using System.Linq;
//using ReeperCommon.Containers;
//using ReeperCommon.Serialization;

//namespace AssemblyReloader.Configuration
//{
//    // Normally the serialization system only selects concrete surrogateSerializer types when searching
//    // for a way to serialize [Target]. If we allowed generic surrogates, there would be no way
//    // to know which instances we'd need to create to handle them in advance
//    //
//    // This extension does exactly that: if we can't serialize target but we have a generic
//    // surrogateSerializer that can, we'll create a surrogateSerializer for that type on request
//    public class GenericSurrogateSelector : DefaultConfigNodeItemSerializerSelector
//    {
//        private readonly ISurrogateProvider _surrogateProvider;

//        public GenericSurrogateSelector(ISurrogateProvider surrogateProvider)
//        {
//            if (surrogateProvider == null) throw new ArgumentNullException("surrogateProvider");
//            _surrogateProvider = surrogateProvider;
//        }


//        public override Maybe<ISurrogateSerializer> GetSurrogate(Type target)
//        {
//            var maybeSurrogate = base.GetSurrogate(target);

//            if (maybeSurrogate.Any()) return maybeSurrogate;

//            // we didn't find a surrogateSerializer for target... let's find all
//            // generic ISurrogateSerializer<T>'s
           
//        }
//    }
//}
