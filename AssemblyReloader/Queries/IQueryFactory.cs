﻿using System.Reflection;
using AssemblyReloader.Queries.AssemblyQueries;
using AssemblyReloader.Queries.ConversionQueries;

namespace AssemblyReloader.Queries
{
    public interface IQueryFactory
    {
        IAddonsFromAssemblyQuery GetAddonsFromAssemblyQuery();
        IAddonAttributeFromTypeQuery GetAddonAttributeQuery();
        IStartupSceneFromGameSceneQuery GetStartupSceneFromGameSceneQuery();
    }
}
