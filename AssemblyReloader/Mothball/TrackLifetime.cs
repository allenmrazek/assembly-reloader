using System;
using System.Linq;
using AssemblyReloader.Messages;
using AssemblyReloader.Messages.Implementation;
using AssemblyReloader.Tracking.Messages;
using ReeperCommon.Extensions;
using UnityEngine;

namespace AssemblyReloader.MonoBehaviours
{
    public enum TrackedItemType
    {
        Addon,
        PartModule,
        ScenarioModule,
        InternalModule
    }

    class TrackLifetime : MonoBehaviour, IConsumer<DestroyAllTrackedObjects>
    {
        [SerializeField] public IMessage StartMessage;

        [SerializeField]
        public IMessage DestroyMessage;

        [SerializeField]
        public IChannel Channel;

        private bool _destroyable = true;



        private void Start()
        {
            print("TrackLifetime started");

            if (!StartMessage.IsNull())
                if (!Channel.IsNull())
                    Channel.Send(StartMessage);

        }



        private void OnDestroy()
        {
            print("TrackLifetime ended: " + gameObject.name);

            if (!DestroyMessage.IsNull())
                if (!Channel.IsNull())
                    Channel.Send(DestroyMessage);
        }



        public void Consume(DestroyAllTrackedObjects message)
        {
            print("TrackLifetime.Consume");

            var ltTrackersOnThisGo = gameObject.GetComponents<TrackLifetime>().ToList();

            if (!message.InvokeDestructionEvent)
                ltTrackersOnThisGo.ForEach(tl => tl.DestroyMessage = tl.StartMessage = null); // if created and destroyed in one frame, start and then destroy will be called

            if (!_destroyable) return; // GameObject already being destroyed; duplicate will cause errors in unity log

            ltTrackersOnThisGo.ForEach(tl => tl._destroyable = false);
            Destroy(gameObject);
        }
    }
}
