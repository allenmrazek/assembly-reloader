using System;
using AssemblyReloader.AddonTracking;

namespace AssemblyReloader.Loaders
{
    class AddonInfo
    {
        public readonly Type type;
        public readonly KSPAddon addon;
        public bool created;

        internal AddonInfo(Type t, KSPAddon add)
        {
            type = t;
            created = false;

            addon = add;
        }

        internal bool RunOnce
        {
            get
            {
                return addon.once;
            }
        }

        internal KSPAddon.Startup Scenes
        {
            get
            {
                return addon.startup;
            }
        }
    }
}