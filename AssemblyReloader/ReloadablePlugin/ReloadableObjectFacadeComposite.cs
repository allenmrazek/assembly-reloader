using System;
using System.Collections.Generic;
using System.Reflection;
using AssemblyReloader.Properties;
using ReeperCommon.FileSystem;

namespace AssemblyReloader.ReloadablePlugin
{
// ReSharper disable once UnusedMember.Global
    public class ReloadableObjectFacadeComposite : IReloadableObjectFacade
    {
        private readonly IEnumerable<IReloadableObjectFacade> _facades;

        public ReloadableObjectFacadeComposite([NotNull] params IReloadableObjectFacade[] facades)
        {
            if (facades == null) throw new ArgumentNullException("facades");
            _facades = facades;
        }


        public void Load([NotNull] Assembly assembly, [NotNull] IFile location)
        {
            if (assembly == null) throw new ArgumentNullException("assembly");
            if (location == null) throw new ArgumentNullException("location");

            foreach (var f in _facades) f.Load(assembly, location);
        }

        public void Unload([NotNull] Assembly assembly, [NotNull] IFile location)
        {
            if (assembly == null) throw new ArgumentNullException("assembly");
            if (location == null) throw new ArgumentNullException("location");

            foreach (var f in _facades) f.Unload(assembly, location);
        }
    }
}
