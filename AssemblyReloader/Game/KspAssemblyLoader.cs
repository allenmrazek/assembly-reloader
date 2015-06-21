using System;
using ReeperCommon.Extensions;

namespace AssemblyReloader.Game
{
    [Implements(typeof(IGameAssemblyLoader))]
    public class KspAssemblyLoader : IGameAssemblyLoader
    {
        public AssemblyLoader.LoadedAssembyList LoadedAssemblies
        {
            get { return AssemblyLoader.loadedAssemblies; }
            set
            {
                if (value.IsNull()) throw new ArgumentNullException("value");

                AssemblyLoader.loadedAssemblies = value;
            }
        }
    }
}
