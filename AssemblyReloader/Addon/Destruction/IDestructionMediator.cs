using UnityEngine;

namespace AssemblyReloader.Addon.Destruction
{
    interface IDestructionMediator
    {
        void InformTargetOfDestruction(GameObject target);
    }
}
