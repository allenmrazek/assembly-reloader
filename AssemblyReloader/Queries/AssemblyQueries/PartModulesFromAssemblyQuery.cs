using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AssemblyReloader.Queries.AssemblyQueries
{
    public class PartModulesFromAssemblyQuery : IPartModulesFromAssemblyQuery
    {
        public IEnumerable<Type> Get(Assembly assembly)
        {
            if (assembly == null) throw new ArgumentNullException("assembly");

            return assembly.GetTypes()
                .Where(ty => ty.IsAssignableFrom(typeof (PartModule)));
        }
    }
}
