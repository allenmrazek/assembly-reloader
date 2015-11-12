extern alias Cecil96;
using System;
using AssemblyReloader.Config.Keys;
using ReeperCommon.FileSystem;
using strange.extensions.command.impl;
using strange.extensions.injector;
using BaseAssemblyResolver = Cecil96::Mono.Cecil.BaseAssemblyResolver;
using DefaultAssemblyResolver = Cecil96::Mono.Cecil.DefaultAssemblyResolver;
using IAssemblyResolver = Cecil96::Mono.Cecil.IAssemblyResolver;

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

        public CommandConfigureCecilAssemblyResolver([Name(DirectoryKey.GameData)] IDirectory gameData)
        {
            if (gameData == null) throw new ArgumentNullException("gameData");
            _gameData = gameData;
        }


        public override void Execute()
        {
            var assemblyResolver = new DefaultAssemblyResolver();

            foreach (var dir in _gameData.RecursiveDirectories())
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
