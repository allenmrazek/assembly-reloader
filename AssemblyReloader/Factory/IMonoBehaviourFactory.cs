using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AssemblyReloader.Factory
{
    interface IMonoBehaviourFactory
    {
        MonoBehaviour Create(Type type, bool track);
        IEnumerable<MonoBehaviour> GetLiveMonoBehaviours();
    }
}
