using System;
using System.Collections.Generic;
using System.Linq;
using AssemblyReloader.AssemblyTracking.Implementations;
using AssemblyReloader.Events;
using AssemblyReloader.Events.Implementations;
using AssemblyReloader.Factory;
using AssemblyReloader.Factory.Implementations;
using AssemblyReloader.GUI;
using AssemblyReloader.Logging;
using AssemblyReloader.Mediators.Destruction;
using AssemblyReloader.Messages;
using AssemblyReloader.Messages.Implementation;
using AssemblyReloader.Providers;
using AssemblyReloader.Queries;
using ReeperCommon.FileSystem;
using ReeperCommon.FileSystem.Implementations;
using ReeperCommon.Gui.Window;
using ReeperCommon.Gui.Window.Decorators;
using ReeperCommon.Gui.Window.Decorators.Buttons;
using ReeperCommon.Gui.Window.Factory;
using ReeperCommon.Gui.Window.View;
using ReeperCommon.Logging;
using ReeperCommon.Logging.Implementations;
using UnityEngine;

namespace AssemblyReloader.MonoBehaviours
{
    // composite root
    [KSPAddon(KSPAddon.Startup.Instantly, true)]
    class Core : MonoBehaviour
    {
        private WindowView _view;
        private ReloadableController _controller;
        private MessageChannel _messageChannel;
        private CurrentGameSceneProvider _currentSceneProvider;
        private IGameEventSubscriber<GameScenes> _eventOnLevelWasLoaded;

        private ILog _log;


        private interface IConsumer
        {
            void Consume(object message);
        }


        private class MessageChannel : IChannel
        {
            private readonly List<IConsumer> _consumers;

            public MessageChannel(params IConsumer[] consumers)
            {
                _consumers = new List<IConsumer>(consumers);
            }


            public void Send<T>(T message)
            {
                _consumers.ForEach(consumer => consumer.Consume(message));
            }

            public void AddListener<T>(object listener)
            {
                AddConsumer(new Consumer<T>(listener));
            }

            public void RemoveListener(object listener)
            {
                _consumers.RemoveAll(ic => ReferenceEquals(ic, listener));
            }


            private void AddConsumer(IConsumer consumer)
            {
                if (consumer == null) throw new ArgumentNullException("consumer");

                if (!_consumers.Contains(consumer))
                    _consumers.Add(consumer);
            }


        }





        private class Consumer<T> : IConsumer
        {
            private readonly object _consumer;

            public Consumer(object consumer)
            {
                if (consumer == null) throw new ArgumentNullException("consumer");
                if (!(consumer is IConsumer<T>)) throw new InvalidOperationException("consumer is not a " + typeof (T).Name);

                _consumer = consumer;
            }


            public void Consume(object message)
            {
                if (message is T && _consumer is IConsumer<T>)
                    (_consumer as IConsumer<T>).Consume((T)message);
            }
        }






        // ReSharper disable once UnusedMember.Local
        private void Start()
        {
            DontDestroyOnLoad(this);

#if DEBUG
            var log = new DebugLog("ART");
#else
            var log = LogFactory.Create(LogLevel.Standard);
#endif

            _log = log.CreateTag("Core");


            var gameDataProvider = new KSPGameDataDirectoryProvider();

            var queryProvider = new QueryProvider();
            _currentSceneProvider = queryProvider.GetCurrentGameSceneProvider();

            var reloaderLog = new ReloaderLog(log, 25);
            var fileFactory = new KSPFileFactory();
            var destructionMediator = new GameObjectDestroyForReload();
            var addonFactory = new AddonFactory(destructionMediator, log.CreateTag("AddonFactory"));

            var infoFactory = new AddonInfoFactory(queryProvider.GetAddonAttributeQuery());

            _eventOnLevelWasLoaded = new GameEventSubscriber<GameScenes>(log.CreateTag("OnLevelWasLoaded"));
                // do not subscribe to GameEvents version of this event because it can be invoked twice in succession due to a KSP bug
                // instead, it is invoked by Core.OnLevelWasLoaded

            var loaderFactory = new LoaderFactory(addonFactory, infoFactory, reloaderLog, queryProvider);









            _log.Normal("Initializing container...");

            var reloadableAssemblyFileQuery = new ReloadableAssemblyFileQuery(
                                                        fileFactory,
                                                        gameDataProvider
                                                        );


            var asm = new ReloadableAssembly(
                reloadableAssemblyFileQuery.Get().First(),
                loaderFactory, 
                _eventOnLevelWasLoaded,
                log.CreateTag("Reloadable:" + reloadableAssemblyFileQuery.Get().First().Name),
                queryProvider);

            asm.Load();
            asm.StartAddons(GameScenes.LOADING);

            _controller = new ReloadableController(queryProvider, reloaderLog, asm);



            //_controller.ReloadAll();


            CreateWindow();

        }



        // ReSharper disable once UnusedMember.Local
        private void OnDestroy()
        {
            _log.Debug("OnDestroy");
        }



        // Unity MonoBehaviour callback
        // ReSharper disable once UnusedMember.Local
        private void OnLevelWasLoaded(int level)
        {
            _eventOnLevelWasLoaded.OnEvent(_currentSceneProvider.Get());
        }



        private void CreateWindow()
        {
            var tempTexture = new Texture2D(16, 16, TextureFormat.ARGB32, false);
            var pixels = Enumerable.Repeat<Color>(new Color(1f, 0f, 0f), 16 * 16);

            tempTexture.SetPixels(pixels.ToArray());
            tempTexture.Apply();

            var style = GUIStyle.none;
            style.active.background = style.normal.background = style.hover.background = tempTexture;

            var wfactory = new WindowFactory(HighLogic.Skin);


            var logic = new ViewLogic(_controller, _log.CreateTag("View"));

            var basicWindow = new BasicWindow(
                logic,
                new Rect(400f, 400f, 300f, 300f),
                2424, HighLogic.Skin,
                true,
                true
                );


            var tbButtons = new TitleBarButtons(basicWindow, new Vector2(4f, 4f));


            tbButtons.AddButton(new TitleBarButton(style, s => { }, "Test"));


            var hiding = new HideOnF2(tbButtons);

            _view = WindowView.Create(hiding);

            DontDestroyOnLoad(_view);

        }
    }
}
