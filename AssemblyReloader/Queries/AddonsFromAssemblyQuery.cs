using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ReeperCommon.Containers;
using ReeperCommon.Extensions;
using UnityEngine;

namespace AssemblyReloader.Queries
{
    class AddonsFromAssemblyQuery
    {
        private readonly Assembly _assembly;
        private readonly AddonAttributeFromTypeQuery _attributeQuery;

        public AddonsFromAssemblyQuery(Assembly assembly, AddonAttributeFromTypeQuery attributeQuery)
        {
            if (assembly == null) throw new ArgumentNullException("assembly");
            if (attributeQuery == null) throw new ArgumentNullException("attributeQuery");
            _assembly = assembly;
            _attributeQuery = attributeQuery;
        }



        public IEnumerable<Type> Get()
        {
            return _assembly
                .GetTypes()
                .Where(t => _attributeQuery.GetKspAddonAttribute(t).Any());
        }
    }
}
