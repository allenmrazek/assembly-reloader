﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssemblyReloader.Game
{
    public interface IVessel
    {
        List<IPart> Parts { get; }
    }
}