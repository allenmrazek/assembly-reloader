using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using AssemblyReloader.Loaders.Addon;
using AssemblyReloader.Providers;
using ReeperCommon.Logging;

namespace AssemblyReloader.Loaders.PartModule
{
    public class PartModuleLoader : IDisposable
    {
        private readonly IEnumerable<Type> _partModules;
        private readonly ILog _log;

        public PartModuleLoader(IEnumerable<Type> partModules, ILog log)
        {
            if (partModules == null) throw new ArgumentNullException("partModules");
            if (log == null) throw new ArgumentNullException("log");

            _partModules = partModules;
            _log = log;
        }


        ~PartModuleLoader()
        {
            Dispose(false);
        }


        public void Dispose()
        {
            Dispose(true);
        }


        private void Dispose(bool managed)
        {
            if (managed)
            {
                // todo: destroy
            }

            GC.SuppressFinalize(this);
        }


        public void LoadPartModulesIntoPrefabs()
        {
            _log.Verbose("Begin loading PartModules into prefab GameObjects");

            foreach (var pm in _partModules)
                LoadPartModuleIntoPrefab(pm);

            _log.Verbose("Finished loading PartModules into prefabs");
        }


        public void LoadPartModulesIntoFlight()
        {
            throw new NotImplementedException();
        }


        private void LoadPartModuleIntoPrefab(Type pm)
        {
            
        }
    }
}
