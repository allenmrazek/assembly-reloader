﻿using System;
using AssemblyReloader.Annotations;
using AssemblyReloader.Commands;

namespace AssemblyReloader.Game
{
    public class DisposeLoadedAssemblyCommandFactory : IDisposeLoadedAssemblyCommandFactory
    {
        public IDisposable Create([NotNull] AssemblyLoader.LoadedAssembly la)
        {
            if (la == null) throw new ArgumentNullException("la");

            return new DisposeLoadedAssembly(la);
        }
    }
}