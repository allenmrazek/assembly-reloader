﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AssemblyReloader.Queries.AssemblyQueries
{
    public interface IPartModulesFromAssemblyQuery
    {
        IEnumerable<Type> Get(Assembly assembly);
    }
}
