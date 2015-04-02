﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AssemblyReloader.Game;

namespace AssemblyReloader.Providers.Game
{
    public interface IProtoScenarioModuleProvider
    {
        IEnumerable<IProtoScenarioModule> Get(Type type);
    }
}