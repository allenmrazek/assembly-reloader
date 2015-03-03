using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AssemblyReloader.DataObjects;
using AssemblyReloader.Loaders.PMLoader;
using AssemblyReloader.Queries;
using AssemblyReloader.Queries.AssemblyQueries;

namespace AssemblyReloader.Providers
{
    public class ProxyPartModuleTypeProvider : IProxyPartModuleTypeProvider
    {
        private readonly IAssemblyProvider<ITypeIdentifier> _proxyAssemblyProvider;
        private readonly ITypesFromAssemblyQuery _partModuleQuery;
        private readonly ITypeIdentifierQuery _identifierQuery;

        public ProxyPartModuleTypeProvider(
            IAssemblyProvider<ITypeIdentifier> proxyAssemblyProvider,
            ITypesFromAssemblyQuery partModuleQuery,
            ITypeIdentifierQuery identifierQuery)
        {
            if (proxyAssemblyProvider == null) throw new ArgumentNullException("proxyAssemblyProvider");
            if (partModuleQuery == null) throw new ArgumentNullException("partModuleQuery");
            if (identifierQuery == null) throw new ArgumentNullException("identifierQuery");
            _proxyAssemblyProvider = proxyAssemblyProvider;
            _partModuleQuery = partModuleQuery;
            _identifierQuery = identifierQuery;
        }


        public Type Get(Type type)
        {
            var proxyAssembly = _proxyAssemblyProvider.Get(_identifierQuery.Get(type));
            if (!proxyAssembly.Any())
                throw new Exception("Failed to get proxy PartModule assembly for type " + type.FullName);

            var asm = proxyAssembly.Single();

            var partModules = _partModuleQuery.Get(asm).ToList();

            if (partModules.Count != 1)
                throw new InvalidOperationException("Number of PartModule prototypes in " + asm.GetName().Name +
                                                    " is unexpected [" + partModules.Count + "]");

            return partModules.Single();
        }
    }
}
