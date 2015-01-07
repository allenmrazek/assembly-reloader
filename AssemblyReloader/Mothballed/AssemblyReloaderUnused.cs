using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReeperCommon.FileSystem;
using ReeperCommon.FileSystem.Implementations;
using ReeperCommon.Logging;
using UnityEngine;

namespace AssemblyReloader
{
    //[KSPAddon(KSPAddon.Startup.Instantly, true)]
    class AssemblyReloaderUnused : MonoBehaviour
    {
        private void Start()
        {
            var log = LogFactory.Create(LogLevel.Debug);

            // hunt down all .reloadable plugins
            IDirectory gamedata = new KSPDirectory(new KSPFileFactory(), new KSPGameDataDirectoryProvider());

            var files = gamedata.RecursiveFiles(".reloadable");

            log.Normal("Found " + files.Count() + " reloadables");
            log.Normal("of " + gamedata.RecursiveFiles().Count() + " total files");
            files.ToList().ForEach(f => log.Normal("Reloadable: " + f.FileName));

        }
    }
}
