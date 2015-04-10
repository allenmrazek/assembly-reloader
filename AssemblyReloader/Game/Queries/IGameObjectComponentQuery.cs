using System;
using System.Collections.Generic;
using UnityEngine;

namespace AssemblyReloader.Game.Queries
{
    public interface IGameObjectComponentQuery
    {
        IEnumerable<Component> Get(Type componentType);
    }
}
