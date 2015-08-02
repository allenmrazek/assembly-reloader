using System;
using System.Reflection;
using AssemblyReloader.Game;
using ReeperCommon.Logging;

namespace AssemblyReloader.ReloadablePlugin.Loaders.PartModules
{
// ReSharper disable once ClassNeverInstantiated.Global
    public class PartModuleFactory : IPartModuleFactory
    {
        private readonly IGetPartIsPrefab _getIsPartPrefab;
        private readonly SignalPartModuleCreated _partModuleCreatedSignal;
        private readonly ILog _log;

        public PartModuleFactory(
            IGetPartIsPrefab getIsPartPrefab,
            SignalPartModuleCreated partModuleCreatedSignal,
            ILog log)
        {
            if (getIsPartPrefab == null) throw new ArgumentNullException("getIsPartPrefab");
            if (partModuleCreatedSignal == null) throw new ArgumentNullException("partModuleCreatedSignal");
            if (log == null) throw new ArgumentNullException("log");

            _getIsPartPrefab = getIsPartPrefab;
            _partModuleCreatedSignal = partModuleCreatedSignal;
            _log = log;
        }


        public void Create(IPart part, PartModuleDescriptor descriptor)
        {
            if (part == null) throw new ArgumentNullException("part");
            if (descriptor == null) throw new ArgumentNullException("descriptor");

            _log.Debug("Creating PartModule " + descriptor.Identifier + " on " + part.FlightID);

            var result = part.GameObject.AddComponent(descriptor.Type) as PartModule;

            if (result == null)
                throw new Exception("Failed to add " + descriptor.Type.FullName + " to " + part.PartName);

            part.Modules.Add(result);


            // if this is the prefab GameObject, it will never become active again and awake will never
            // get called so we must do it ourselves
            if (_getIsPartPrefab.Get(part))
            {
                var method = typeof (PartModule).GetMethod("Awake",
                    BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);

                if (method != null)
                    method.Invoke(result, null);
            }

            _partModuleCreatedSignal.Dispatch(part, result, descriptor);
        }
    }
}
