using AssemblyReloader.Messages;
using AssemblyReloader.Messages.Implementation;
using ReeperCommon.Extensions;
using UnityEngine;

namespace AssemblyReloader.AddonTracking
{
    class AddonLifetimeTracker : MonoBehaviour
    {
        public IChannel messageChannel;

        private void Start()
        {
            if (!messageChannel.IsNull())
                messageChannel.Send(new AddonCreated(gameObject, this));
        }

        private void OnDestroy()
        {
            if (!messageChannel.IsNull())
                messageChannel.Send(new AddonDestroyed(gameObject, this));
        }
    }
}
