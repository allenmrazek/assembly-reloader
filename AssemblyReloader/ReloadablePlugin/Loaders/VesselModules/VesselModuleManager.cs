using System;
using ReeperCommon.Containers;
using ReeperCommon.Logging;

namespace AssemblyReloader.ReloadablePlugin.Loaders.VesselModules
{
// ReSharper disable once UnusedMember.Global
    public class KspVesselModuleManager : IVesselModuleManager
    {
        private readonly IKspFactory _kspFactory;

        public KspVesselModuleManager(IKspFactory kspFactory)
        {
            if (kspFactory == null) throw new ArgumentNullException("kspFactory");

            _kspFactory = kspFactory;
        }


        public Maybe<VesselModuleManager.VesselModuleWrapper> GetModuleWrapper(VesselModuleType type)
        {
            if (type == null) throw new ArgumentNullException("type");

            var log = new DebugLog("VesselModuleManager");

            foreach (var m in VesselModuleManager.Modules)
                log.Normal("Module: " + m.type.FullName);

            return VesselModuleManager.GetWrapper(type.Type)
                .ToMaybe();
        }


        public void AddWrapper(VesselModuleManager.VesselModuleWrapper wrapper)
        {
            if (wrapper == null) throw new ArgumentNullException("wrapper");

            VesselModuleManager.Modules.Add(wrapper);
        }


        public void RemoveWrapper(VesselModuleManager.VesselModuleWrapper wrapper)
        {
            if (wrapper == null) throw new ArgumentNullException("wrapper");

            if (!VesselModuleManager.Modules.Remove(wrapper))
                throw new VesselModuleWrapperNotFoundException(wrapper);
        }
    }
}
