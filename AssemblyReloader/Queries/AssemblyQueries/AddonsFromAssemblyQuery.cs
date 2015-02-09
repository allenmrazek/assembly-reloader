using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AssemblyReloader.Queries.ConversionQueries;
using UnityEngine;

namespace AssemblyReloader.Queries.AssemblyQueries
{
    public class AddonsFromAssemblyQuery : IAddonsFromAssemblyQuery
    {
        private readonly IAddonAttributeFromTypeQuery _attributeQuery;

        public AddonsFromAssemblyQuery(IAddonAttributeFromTypeQuery attributeQuery)
        {
            if (attributeQuery == null) throw new ArgumentNullException("attributeQuery");

            _attributeQuery = attributeQuery;
        }



        public IEnumerable<Type> Get(Assembly assembly)
        {
            if (assembly == null) throw new ArgumentNullException("assembly");

            return assembly
                .GetTypes()
                .Where(t => _attributeQuery.Get(t).Any())
                .Where(t => typeof(MonoBehaviour).IsAssignableFrom(t) 
                    && !typeof(PartModule).IsAssignableFrom(t) 
                    && !typeof(ScenarioModule).IsAssignableFrom(t)); // don't let KSPAddon-marked PartModules/SModules get selected (they're likely errors anyway)
        }
    }
}
