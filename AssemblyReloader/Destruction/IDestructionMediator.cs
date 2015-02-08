using UnityEngine;

namespace AssemblyReloader.Destruction
{
    public interface IDestructionMediator
    {
        void InformTargetOfDestruction(GameObject target);
        void InformComponentOfDestruction(Component component);
    }
}
