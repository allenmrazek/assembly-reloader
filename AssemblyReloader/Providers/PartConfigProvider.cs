using System.Linq;
using ReeperCommon.Containers;

namespace AssemblyReloader.Providers
{
    public class PartConfigProvider : IPartConfigProvider
    {
        public Maybe<ConfigNode> Get(AvailablePart availablePart)
        {
            var found = GameDatabase.Instance.GetConfigs("PART")
                .FirstOrDefault(u => u.name.Replace('_', '.') == availablePart.name);

            return found == null ? Maybe<ConfigNode>.None : Maybe<ConfigNode>.With(found.config);
        }
    }
}
