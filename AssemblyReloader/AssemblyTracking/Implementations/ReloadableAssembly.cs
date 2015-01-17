using System;
using System.Collections.Generic;
using System.Reflection;
using AssemblyReloader.Factory;
using AssemblyReloader.ILModifications;
using AssemblyReloader.Loaders;
using AssemblyReloader.Providers;
using AssemblyReloader.Tracking;
using Mono.Cecil;
using ReeperCommon.Extensions;
using ReeperCommon.FileSystem;
using ReeperCommon.Logging;

namespace AssemblyReloader.AssemblyTracking.Implementations
{

    /// <summary>
    /// This object tracks a particular reloadable dll (based on its location). This is necessary
    /// because the state of a previous version of the assembly may affect how we load the next one;
    /// specifically, we may need to modify changes the previous version made such as removing PartModules
    /// from prefabs
    /// </summary>
    class ReloadableAssembly : IReloadableAssembly
    {
        private System.Reflection.Assembly _loaded;
        private IAddonLoader _addonLoader;

        private readonly IFile _file;
        private readonly LoaderFactory _loaderFactory;
        private readonly ILog _log;
        private readonly QueryProvider _queryProvider;


        public ReloadableAssembly(
            IFile file,
            LoaderFactory loaderFactory,
            ILog log,
            QueryProvider queryProvider)
        {
            if (file == null) throw new ArgumentNullException("file");
            if (loaderFactory == null) throw new ArgumentNullException("loaderFactory");
            if (log == null) throw new ArgumentNullException("log");
            if (queryProvider == null) throw new ArgumentNullException("queryProvider");

            _file = file;
            _loaderFactory = loaderFactory;
            _log = log;
            _queryProvider = queryProvider;
        }




        private void ApplyModifications(System.IO.MemoryStream stream)
        {
            var definition = AssemblyDefinition.ReadAssembly(_file.FullPath);

            var modifier = new ModifyPluginIdentity();

            modifier.Rename(definition, Guid.NewGuid());

            _log.Normal("Finished modifications; writing to stream");
            definition.Write(stream);
            //definition.Write(_file.FullPath + ".edit");
            _log.Normal("done");
        }





        public void Load()
        {
            using (var stream = new System.IO.MemoryStream())
            {
                ApplyModifications(stream); // modified assembly written to memory

                // load dll from byte stream. This is done simply to avoid unnecessary file i/o;
                // File.ReadAllBytes works too but then we waste time writing a file and then immediately
                // reading it again a single time

                _log.Normal("loading assembly");
                _loaded = Assembly.Load(stream.GetBuffer());
                _log.Normal("load finished");
                if (_loaded.IsNull())
                    throw new InvalidOperationException("Failed to load byte stream as Assembly");

                _addonLoader = _loaderFactory.GetAddonLoader(_queryProvider.GetAddonsFromAssemblyQuery(_loaded).Get());
            }
        }



        public void Unload()
        {
            _addonLoader.Deinitialize();
            _addonLoader.Dispose();
        }



        public void StartAddons(GameScenes scene)
        {
            _addonLoader.LoadAddonsForScene(scene);
        }
    }
}
