﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssemblyReloader.Generators
{
    public class UniqueFlightIdGenerator : IUniqueFlightIdGenerator
    {
        private static uint _internalCounter = 1;

        public uint Get()
        {
            return HighLogic.CurrentGame != null ? ShipConstruction.GetUniqueFlightID(HighLogic.CurrentGame.Updated().flightState) : _internalCounter++;
        }
    }
}