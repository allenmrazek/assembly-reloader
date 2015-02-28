//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using AssemblyReloader.DataObjects;
//using AssemblyReloader.Loaders.PMLoader;
//using AssemblyReloader.Queries;
//using AssemblyReloader.Queries.AssemblyQueries;

//namespace AssemblyReloader.Providers
//{
//    public class ProxyPartModuleTypeProvider : IProxyPartModuleTypeProvider
//    {
//        private readonly IAssemblyProvider<ITypeIdentifier> _proxyAssemblyProvider;
//        private readonly IPartModulesFromAssemblyQuery _partModuleQuery;
//        private readonly ITypeIdentifierQuery _identifierQuery;

//        public ProxyPartModuleTypeProvider(
//            IAssemblyProvider<ITypeIdentifier> proxyAssemblyProvider,
//            IPartModulesFromAssemblyQuery partModuleQuery,
//            ITypeIdentifierQuery identifierQuery)
//        {
//            if (proxyAssemblyProvider == null) throw new ArgumentNullException("proxyAssemblyProvider");
//            if (partModuleQuery == null) throw new ArgumentNullException("partModuleQuery");
//            if (identifierQuery == null) throw new ArgumentNullException("identifierQuery");
//            _proxyAssemblyProvider = proxyAssemblyProvider;
//            _partModuleQuery = partModuleQuery;
//            _identifierQuery = identifierQuery;
//        }


//        public Type Get(PartModuleDescriptor descriptor)
//        {
//            var proxyAssembly = _proxyAssemblyProvider.Get(_identifierQuery.Get(descriptor.Type));

//            var partModules = _partModuleQuery.Get(proxyAssembly).ToList();

//            if (partModules.Count != 1)
//                throw new InvalidOperationException("Number of PartModule prototypes in " + proxyAssembly.GetName().Name +
//                                                    " is unexpected [" + partModules.Count + "]");

//            return partModules.Single();
//        }
//    }
//}
