using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using AssemblyReloader.AddonTracking;
using AssemblyReloader.Queries;

namespace AssemblyReloader.Factory
{
    class QueryProvider
    {
        private readonly AddonAttributeFromTypeQuery _attributeQuery;
        private readonly StartupSceneFromGameSceneQuery _startupSceneConverter;



        public QueryProvider()
        {
            _attributeQuery = new AddonAttributeFromTypeQuery();
        }



        public AddonsFromAssemblyQuery GetAddonsFromAssemblyQuery(Assembly assembly)
        {
            if (assembly == null) throw new ArgumentNullException("assembly");

            return new AddonsFromAssemblyQuery(assembly, _attributeQuery);
        }



        public AddonAttributeFromTypeQuery GetAddonAttributeQuery()
        {
            return _attributeQuery;
        }


        public StartupSceneFromGameSceneQuery GetStartupSceneFromGameSceneQuery()
        {
            return _startupSceneConverter;
        }
    }
}
