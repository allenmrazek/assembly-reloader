using System;
using AssemblyReloader.Game;

namespace AssemblyReloader.Queries
{
    public class PartIsPrefabQuery : IPartIsPrefabQuery
    {
        public bool Get(IPart part)
        {
            if (part == null) throw new ArgumentNullException("part");

            return ReferenceEquals(part.GameObject, part.PartInfo.PartPrefab.GameObject);
        }
    }
}
