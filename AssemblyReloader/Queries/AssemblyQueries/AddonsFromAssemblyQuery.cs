using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace AssemblyReloader.Queries.AssemblyQueries
{
    public class AddonsFromAssemblyQuery : ITypesFromAssemblyQuery
    {
        private readonly IAddonAttributesFromTypeQuery _attributesQuery;

        public AddonsFromAssemblyQuery(IAddonAttributesFromTypeQuery attributesQuery)
        {
            if (attributesQuery == null) throw new ArgumentNullException("attributesQuery");

            _attributesQuery = attributesQuery;
        }



        public IEnumerable<Type> Get(Assembly assembly)
        {
            if (assembly == null) throw new ArgumentNullException("assembly");

            return assembly
                .GetTypes()
                .Where(t => _attributesQuery.Get(t).Any())
                .Where(t => typeof(MonoBehaviour).IsAssignableFrom(t) 
                    && !typeof(PartModule).IsAssignableFrom(t) 
                    && !typeof(ScenarioModule).IsAssignableFrom(t)); // don't let KSPAddon-marked PartModules/SModules get selected (they're likely errors anyway)
        }
    }
}
