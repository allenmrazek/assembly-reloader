using System;
using System.Collections.Generic;
using System.Linq;
using AssemblyReloader.AddonTracking;
using AssemblyReloader.Factory;
using AssemblyReloader.GUI;
using AssemblyReloader.Logging;
using AssemblyReloader.Messages;
using AssemblyReloader.Messages.Implementation;
using AssemblyReloader.Providers;
using AssemblyReloader.Queries;
using AssemblyReloader.Services;
using AssemblyReloader.Tracking.Messages;
using ReeperCommon.FileSystem;
using ReeperCommon.FileSystem.Implementations;
using ReeperCommon.Gui.Window;
using ReeperCommon.Gui.Window.Decorators;
using ReeperCommon.Gui.Window.Factory;
using ReeperCommon.Gui.Window.View;
using ReeperCommon.Logging;
using UnityEngine;

namespace AssemblyReloader.MonoBehaviours
{
    // composite root
    [KSPAddon(KSPAddon.Startup.Instantly, true)]
    class Core : MonoBehaviour
    {
        private WindowView _view;
        private ReloadableContainer _container;



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


        class TestConsumer : IConsumer<TestEvent>
        {
            private readonly Log _log;

            public TestConsumer(Log log)
            {
                if (log == null) throw new ArgumentNullException("log");
                _log = log;
            }


            public void Consume(object message)
            {
                _log.Warning("GOT THE (generic) MESSAGE!");
            }

            public void Consume(TestEvent message)
            {
                _log.Warning("GOT THE MESSAGE!");
            }
        }



        private void Start()
        {
            DontDestroyOnLoad(this);

#if DEBUG
            var log = LogFactory.Create(LogLevel.Debug);
#else
            var log = LogFactory.Create(LogLevel.Standard);
#endif
            

            log.Normal("Testing it out ...");

            var msg = new MessageChannel(new Consumer<TestEvent>(new TestConsumer(log)));
            //msg.AddConsumer(new Consumer<TestEvent>(new TestConsumer(log)));
            //msg.AddConsumer(new Consumer<AddonCreated>(new OtherConsumer(log)));
            msg.AddListener<TestEvent>(new TestConsumer(log));

            msg.Send(new TestEvent());
            log.Normal("test complete");

            log.Normal("Testing duplication mechanics");
            var lifetimeService = new GameObjectLifetimeService(msg);

            var testGo = new GameObject();

            log.Normal("registering");
            lifetimeService.RegisterAddon(testGo);
            log.Normal("destroying");

            Destroy(testGo);

            log.Normal("destroy sent");

            var gameDataProvider = new KSPGameDataDirectoryProvider();
            var startupSceneFromGameScene = new StartupSceneFromGameSceneQuery();

            var currentSceneProvider = new CurrentStartupSceneProvider(startupSceneFromGameScene);

            var reloadableAssemblyFactory = new Factory.ReloadableAssemblyFactory(new AddonsFromAssemblyQuery());

            var reloaderLog = new ReloaderLog(log, 25);
            var fileFactory = new KSPFileFactory();

            var loaderFactory = new LoaderFactory(reloaderLog, currentSceneProvider);

            var reloadableAssemblyFileQuery = new ReloadableAssemblyFileQuery(
                fileFactory,
                gameDataProvider
                );

            

            _container = new ReloadableContainer(
                                                    reloadableAssemblyFactory, 
                                                    loaderFactory,
                                                    reloadableAssemblyFileQuery,
                                                    startupSceneFromGameScene,
                                                    reloaderLog
                                                );

            GameEvents.onLevelWasLoaded.Add(_container.HandleLevelLoad);

            log.Normal("Initializing container...");

            _container.Initialize();

            CreateWindow();
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


            var logic = new ViewLogic(LogFactory.Create(LogLevel.Debug));

            var basicWindow = new BasicWindow(
                logic,
                new Rect(400f, 400f, 300f, 300f),
                2424, HighLogic.Skin,
                true,
                true
                );


            var tbButtons = new TitleBarButtons(basicWindow, new Vector2(4f, 4f));
            tbButtons.AddButton(style, (string str) =>
            {
            }
        , "Test");

            var hiding = new HideOnF2(tbButtons);

            _view = WindowView.Create(hiding);

            DontDestroyOnLoad(_view);


            IDirectory gamedata = new KSPDirectory(new KSPFileFactory(), new KSPGameDataDirectoryProvider());
            //var ra = new ReloadableAssembly(gamedata.RecursiveFiles("reloadable").Single(),
            //    LogFactory.Create(LogLevel.Debug));
        }
    }
}
