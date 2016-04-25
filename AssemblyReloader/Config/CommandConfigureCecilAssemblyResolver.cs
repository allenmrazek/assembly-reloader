extern alias Cecil96;
using System;
using System.Linq;
using AssemblyReloader.Config.Keys;
using Cecil96::Mono.Cecil;
using ReeperCommon.Logging;
using ReeperKSP.FileSystem;
using strange.extensions.command.impl;

namespace AssemblyReloader.Config
{
    /// <summary>
    /// Cecil's resolver will be used to resolve assemblies on disk (NOT in memory) when a TypeDefinition or
    /// TypeReference refers to another assembly's types. Therefore, we need to make sure we search all possible
    /// locations for plugins.
    /// 
    /// Reeper
    /// </summary>
// ReSharper disable once ClassNeverInstantiated.Global
    public class CommandConfigureCecilAssemblyResolver : Command
    {
        private readonly IDirectory _gameData;
        private readonly ILog _log;

        public CommandConfigureCecilAssemblyResolver([Name(DirectoryKey.GameData)] IDirectory gameData, ILog log)
        {
            if (gameData == null) throw new ArgumentNullException("gameData");
            if (log == null) throw new ArgumentNullException("log");
            _gameData = gameData;
            _log = log;
        }


        public override void Execute()
        {
            var assemblyResolver = new DefaultAssemblyResolver();

            foreach (var dir in _gameData.RecursiveDirectories().Union(new [] { _gameData })) // GD doesn't include itself in RecursiveDirectories()
                assemblyResolver.AddSearchDirectory(dir.FullPath);

            // todo: also search legacy locations?

            injectionBinder
                .Bind<BaseAssemblyResolver>()
                .Bind<DefaultAssemblyResolver>()
                .Bind<IAssemblyResolver>()
                    .ToValue(assemblyResolver).CrossContext();
        }
    }
}
