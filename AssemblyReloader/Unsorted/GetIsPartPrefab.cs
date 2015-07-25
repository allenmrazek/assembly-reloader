﻿using System;
using AssemblyReloader.Game;

namespace AssemblyReloader.Unsorted
{
    public class GetIsPartPrefab : IGetIsPartPrefab
    {
        public bool Get(IPart part)
        {
            if (part == null) throw new ArgumentNullException("part");

            return ReferenceEquals(part.GameObject, part.PartInfo.PartPrefab.GameObject);
        }
    }
}