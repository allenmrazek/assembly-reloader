using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using AssemblyReloader.ILModifications;
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
    class ReloadableAssembly
    {
        private System.Reflection.Assembly _loaded;
        private readonly IFile _file;
        private readonly Log _log;





        public ReloadableAssembly(IFile file, Log log)
        {
            if (file == null) throw new ArgumentNullException("file");
            if (log == null) throw new ArgumentNullException("log");

            _file = file;
            _log = log;

            Load();
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



        public void Unload()
        {

            // todo: PartModules

            // todo: ScenarioModules

            // todo: InternalModules (?)

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


                
            }
        }



        

        public IEnumerable<Type> Types { get { return _loaded.GetTypes(); }}
        public Assembly Loaded { get { return _loaded; }}
        
        
        //private void LoadKSPAddon()
        //{
        //    _log.Normal("loading kspaddons");

        //    // identify all KSPAddons in this type
        //    var found = _loaded.GetTypes()
        //                    .Where(ty =>
        //                        ty.IsClass && 
        //                        ty.IsSubclassOf(typeof(MonoBehaviour)) &&
        //                        ty.GetCustomAttributes(true).Any(attr => attr.GetType().IsAssignableFrom(typeof(KSPAddon))));

        //    // if we already have loaded types from the list (which is highly likely), we might need
        //    // to deal with unloading them
        //    var loaded = found.Where(fType => _addonList.Any(t => t.FullName == fType.FullName)).ToList();

        //    // now, some types might no longer exist. Those need to be unloaded as well
        //    var loadedButDeleted = _addonList.Where(addon => !found.Any(f => addon.FullName == f.FullName)).ToList();


        //    _log.Normal("print already loaded...");
        //    loaded.ForEach(l => _log.Normal("Already loaded and needs unload: " + l.FullName));
        //    _log.Normal("done");

        //    // todo: unload found items

        //    // todo: unload deleted items
            
            
        //    // todo: load new items

        //    // and update state so we've got a list of all items loaded in memory again
        //    _addonList = found.ToList();


        //    found.ToList().ForEach(f => _log.Normal("KSPAddon: " + f.FullName));


        //}
    }
}
