using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using AssemblyReloader.Providers.SceneProviders;
using ReeperCommon.FileSystem;

namespace AssemblyReloader.Controllers
{
    public class AddonController : IAddonController
    {
        private readonly ICurrentStartupSceneProvider _currentSceneProvider;

        public AddonController(
            ICurrentStartupSceneProvider currentSceneProvider
            )
        {
            if (currentSceneProvider == null) throw new ArgumentNullException("currentSceneProvider");
            _currentSceneProvider = currentSceneProvider;
        }


        public void StartAddonsFrom(Assembly assembly, IFile location)
        {
            throw new NotImplementedException();
        }


        public void DestroyAddonsFrom(Assembly assembly, IFile location)
        {
            throw new NotImplementedException();
        }
    }
}
