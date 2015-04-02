﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReeperCommon.Containers;

namespace AssemblyReloader.Game
{
    public interface IGameObjectProvider
    {
        Maybe<ScenarioRunner> Get();
    }
}