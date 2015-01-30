using System;
using UnityEngine;

namespace AssemblyReloader.Mediators.Implementations
{
    // inform the target (generic GameObject) that we'll be destroying it to make way for
    // a new instance from an assembly that is about to be loaded
    class GameObjectDestroyForReload : IDestructionMediator
    {
        public void InformTargetOfDestruction(GameObject target)
        {
            if (target == null) throw new ArgumentNullException("target");

            target.SendMessage("OnAssemblyReloadRequested");
        }
    }
}
