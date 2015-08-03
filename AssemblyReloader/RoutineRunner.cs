﻿using System;
using System.Collections;
using AssemblyReloader.Config.Keys;
using AssemblyReloader.StrangeIoC.extensions.implicitBind;
using AssemblyReloader.StrangeIoC.extensions.injector;
using AssemblyReloader.StrangeIoC.extensions.injector.api;
using UnityEngine;

namespace AssemblyReloader
{
    [Implements(typeof(IRoutineRunner), InjectionBindingScope.CROSS_CONTEXT)]
// ReSharper disable once UnusedMember.Global
    public class RoutineRunner : IRoutineRunner
    {
        private readonly GameObject _gameObject;
        private MonoBehaviour _monoBehaviour;

// ReSharper disable once ClassNeverInstantiated.Local
        private class Runner : MonoBehaviour
        {
            
        }


        public RoutineRunner([Name(GameObjectKeys.CoreContext)] GameObject gameObject)
        {
            if (gameObject == null) throw new ArgumentNullException("gameObject");

            _gameObject = gameObject;
        }


        [PostConstruct]
// ReSharper disable once UnusedMember.Global
        public void Initialize()
        {
            _monoBehaviour = _gameObject.AddComponent<Runner>();
        }


        public Coroutine StartCoroutine(IEnumerator coroutine)
        {
            if (coroutine == null) throw new ArgumentNullException("coroutine");

            return _monoBehaviour.StartCoroutine(coroutine);
        }
    }
}