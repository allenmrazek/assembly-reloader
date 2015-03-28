using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AssemblyReloader.Game;
using ReeperCommon.Containers;

namespace AssemblyReloader.Providers.Game
{
    public interface ICurrentGameProvider
    {
        Maybe<IGame> Get();
    }
}
