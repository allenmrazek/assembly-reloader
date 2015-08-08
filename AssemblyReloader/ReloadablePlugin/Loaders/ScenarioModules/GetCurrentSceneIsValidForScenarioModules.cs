﻿using System;
using System.Linq;
using AssemblyReloader.ReloadablePlugin.Loaders.Addons;
using AssemblyReloader.StrangeIoC.extensions.implicitBind;
using AssemblyReloader.StrangeIoC.extensions.injector.api;

namespace AssemblyReloader.ReloadablePlugin.Loaders.ScenarioModules
{
    [Implements(typeof(IGetCurrentSceneIsValidForScenarioModules), InjectionBindingScope.CROSS_CONTEXT)]
// ReSharper disable once UnusedMember.Global
    public class GetCurrentSceneIsValidForScenarioModules : IGetCurrentSceneIsValidForScenarioModules
    {
        private static readonly GameScenes[] ValidScenes =
        {
            GameScenes.SPACECENTER,
            GameScenes.FLIGHT,
            GameScenes.SPACECENTER, 
            GameScenes.TRACKSTATION,
            GameScenes.EDITOR
        };

        private readonly IGetCurrentGameScene _gameSceneQuery;

        public GetCurrentSceneIsValidForScenarioModules(IGetCurrentGameScene gameSceneQuery)
        {
            if (gameSceneQuery == null) throw new ArgumentNullException("gameSceneQuery");
            _gameSceneQuery = gameSceneQuery;
        }


        public bool Get()
        {
            return ValidScenes.Contains(_gameSceneQuery.Get());
        }
    }
}
