using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AssemblyReloader.Mediators
{
    interface IDestructionMediator
    {
        void InformTargetOfDestruction(GameObject target);
    }
}
