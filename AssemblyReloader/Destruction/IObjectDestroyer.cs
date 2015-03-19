using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Contracts;
using UnityEngine;

namespace AssemblyReloader.Destruction
{
    public interface IObjectDestroyer
    {
        void Destroy(MonoBehaviour mb);
        void Destroy(PartModule pm);
        void Destroy(ScenarioModule sm);
        void Destroy(InternalModule im);
        void Destroy(Contract contract);
        // todo: experienceTrait?
    }
}
