using System;
using UnityEngine;

namespace AssemblyReloader.Destruction
{
    // inform the target (generic GameObject) that we'll be destroying it to make way for
    // a new instance from an assembly that is about to be loaded
    // bug: this will only work on active GameObjects
    class GameObjectDestroyForReload : IDestructionMediator
    {
        public void InformTargetOfDestruction(GameObject target)
        {
            if (target == null) throw new ArgumentNullException("target");

            target.SendMessage("OnAssemblyReloadRequested");
        }

        public void InformComponentOfDestruction(Component component)
        {
            if (component == null) throw new ArgumentNullException("component");

            component.SendMessage("OnAssemblyReloadRequested", SendMessageOptions.DontRequireReceiver);
        }
    }
}
