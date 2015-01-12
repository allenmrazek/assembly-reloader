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
        public IMessage StartMessage;
        public IMessage DestroyMessage;
        public IChannel Channel;
        private bool _destroyable = true;



        //public static TrackLifetime Track(
        //    GameObject go,
        //    IChannel channel,
        //    IMessage startMessage,
        //    IMessage destroyMessage)
        //{
        //    if (go == null) throw new ArgumentNullException("go");
        //    if (channel == null) throw new ArgumentNullException("channel");

        //    var tracker = go.AddComponent<TrackLifetime>();

        //    tracker.Channel = channel;
        //    tracker.StartMessage = startMessage;
        //    tracker.DestroyMessage = destroyMessage;

        //    return tracker;
        //}



        private void Start()
        {
            print("TrackLifetime started");

            if (!StartMessage.IsNull())
                if (!Channel.IsNull())
                    Channel.Send(StartMessage);

        }



        private void OnDestroy()
        {
            print("TrackLifetime ended");

            if (!DestroyMessage.IsNull())
                if (!Channel.IsNull())
                    Channel.Send(DestroyMessage);
        }



        public void Consume(DestroyAllTrackedObjects message)
        {
            var ltTrackersOnThisGo = gameObject.GetComponents<TrackLifetime>().ToList();

            if (!message.InvokeDestructionEvent)
                ltTrackersOnThisGo.ForEach(tl => tl.DestroyMessage = null);

            if (!_destroyable) return; // GameObject already being destroyed; duplicate will cause errors in unity log

            ltTrackersOnThisGo.ForEach(tl => tl._destroyable = false);
            Destroy(gameObject);
        }
    }
}
