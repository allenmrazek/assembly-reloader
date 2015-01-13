using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using AssemblyReloader.Factory;
using AssemblyReloader.ILModifications;
using AssemblyReloader.Loaders;
using AssemblyReloader.Providers;
using AssemblyReloader.Queries;
using AssemblyReloader.Tracking;
using Mono.Cecil;
using ReeperCommon.Extensions;
using ReeperCommon.FileSystem;
using ReeperCommon.Logging;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AssemblyReloader.AddonTracking
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
        private readonly IFile _file;
        private readonly LoaderFactory _loaderFactory;
        private List<ILoader> _loaders;
        private readonly Log _log;
        private readonly QueryProvider _queryProvider;


        public ReloadableAssembly(
            IFile file, 
            LoaderFactory loaderFactory,
            Log log,
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



        ~ReloadableAssembly()
        {
            _log.Debug("Reloadable assembly " + _file.FileName + " destructing");
            //throw new Exception("can you see me?");
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

            //using (var stream = new System.IO.MemoryStream())
            //{
            //    ApplyModifications(stream); // modified assembly written to memory

            //    // load dll from byte stream. This is done simply to avoid unnecessary file i/o;
            //    // File.ReadAllBytes works too but then we waste time writing a file and then immediately
            //    // reading it again a single time

            //    _log.Normal("loading assembly");
            //    _loaded = Assembly.Load(stream.GetBuffer());
            //    _log.Normal("load finished");
            //    if (_loaded.IsNull())
            //        throw new InvalidOperationException("Failed to load byte stream as Assembly");


                
            //}
        }



        public void Unload()
        {
            _loaders.ForEach(il => il.Dispose());
            _loaders.Clear();
        }



  
    }
}
