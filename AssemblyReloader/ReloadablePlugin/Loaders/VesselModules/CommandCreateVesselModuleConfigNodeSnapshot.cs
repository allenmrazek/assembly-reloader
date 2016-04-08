using System;
using ReeperCommon.Containers;
using ReeperCommon.Logging;
using strange.extensions.command.impl;
using UnityEngine;

namespace AssemblyReloader.ReloadablePlugin.Loaders.VesselModules
{
    // ReSharper disable once ClassNeverInstantiated.Global
    class CommandCreateVesselModuleConfigNodeSnapshot : Command
    {
        private readonly MonoBehaviour _mbBeingDestroyed;
        private readonly IVesselModuleConfigNodeRepository _repository;
        private readonly IGetTypeIdentifier _typeIdentifierQuery;

        public CommandCreateVesselModuleConfigNodeSnapshot(
            MonoBehaviour mbBeingDestroyed,
            IVesselModuleConfigNodeRepository repository, 
            IGetTypeIdentifier typeIdentifierQuery)
        {
            if (mbBeingDestroyed == null) throw new ArgumentNullException("mbBeingDestroyed");
            if (repository == null) throw new ArgumentNullException("repository");
            if (typeIdentifierQuery == null) throw new ArgumentNullException("typeIdentifierQuery");
            _mbBeingDestroyed = mbBeingDestroyed;
            _repository = repository;
            _typeIdentifierQuery = typeIdentifierQuery;
        }


        public override void Execute()
        {
            Log.Debug("Creating snapshot of VesselModule " +
                      (_mbBeingDestroyed != null ? _mbBeingDestroyed.GetType().FullName : "<null>"));

            if (!(_mbBeingDestroyed is VesselModule))
                return;

            try
            {
                var vesselModule = (VesselModule)_mbBeingDestroyed;
                var vesselModuleTypeName = vesselModule.With(vm => vm.GetType()).Name;
                var snapshot = new ConfigNode(vesselModuleTypeName);

                vesselModule.gameObject.GetComponent<Vessel>()
                    .Do(v => vesselModule.Save(snapshot))
                    .Do(v => _repository.Store(_typeIdentifierQuery.Get(vesselModule.With(vm => vm.GetType())), v.id, snapshot))
                    .IfNull(() => Log.Error("Could not find vessel that " + vesselModuleTypeName + " is attached to"));

            }
            // ReSharper disable once CatchAllClause
            catch (Exception e)
            {
                Log.Warning("Failed to create snapshot of " +
                            _mbBeingDestroyed.With(mb => mb.GetType().FullName).ToMaybe().Or("<null VesselModule>"));
                Log.Warning("This VesselModule will not have its ConfigNode reloaded without a scene change.");
                Log.Warning("Exception received: " + e);
            }
        }
    }
}
