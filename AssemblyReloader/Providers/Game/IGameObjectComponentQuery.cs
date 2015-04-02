using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AssemblyReloader.Providers.Game
{
    public interface IGameObjectComponentQuery
    {
        IEnumerable<Component> Get(Type componentType);
    }
}
