using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AssemblyReloader.AssemblyTracking.Implementations;
using AssemblyReloader.Events;
using AssemblyReloader.Events.Implementations;
using AssemblyReloader.Factory;
using AssemblyReloader.Factory.Implementations;
using AssemblyReloader.GUI;
using AssemblyReloader.Logging;
using AssemblyReloader.Logging.Implementations;
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
using ReeperCommon.Gui.Window.Providers;
using ReeperCommon.Locators.Resources;
using ReeperCommon.Locators.Resources.Implementations;
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
        private WindowView _logView;

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
            var primaryLog = new DebugLog("ART");
#else
            var primaryLog = LogFactory.Create(LogLevel.Standard);
#endif

            var cachedLog = new CachedLog(primaryLog, 100);

            _log = cachedLog;


            var gameDataProvider = new KSPGameDataDirectoryProvider();

            var queryProvider = new QueryProvider();
            _currentSceneProvider = queryProvider.GetCurrentGameSceneProvider();


            var resourceProvider = new ResourceProvider(
                                        new ResourceLocatorComposite(
                                            new ResourceFromEmbeddedResource(Assembly.GetExecutingAssembly())
                                        )
                                    );


            var fileFactory = new KSPFileFactory();
            var destructionMediator = new GameObjectDestroyForReload();
            var addonFactory = new AddonFactory(destructionMediator, cachedLog.CreateTag("AddonFactory"));

            var infoFactory = new AddonInfoFactory(queryProvider.GetAddonAttributeQuery());

            _eventOnLevelWasLoaded = new GameEventSubscriber<GameScenes>(cachedLog.CreateTag("OnLevelWasLoaded"));
                // do not subscribe to GameEvents version of this event because it can be invoked twice in succession due to a KSP bug
                // instead, it is invoked by Core.OnLevelWasLoaded

            var loaderFactory = new LoaderFactory(addonFactory, infoFactory, cachedLog, queryProvider);









            _log.Normal("Initializing container...");

            var reloadableAssemblyFileQuery = new ReloadableAssemblyFileQuery(
                                                        fileFactory,
                                                        gameDataProvider
                                                        );


            var asm = new ReloadableAssembly(
                new ReloadableReloadableIdentity(reloadableAssemblyFileQuery.Get().First()),
                loaderFactory, 
                _eventOnLevelWasLoaded,
                cachedLog.CreateTag("Reloadable:" + reloadableAssemblyFileQuery.Get().First().Name),
                queryProvider);

            asm.Load();
            asm.StartAddons(GameScenes.LOADING);

            _controller = new ReloadableController(queryProvider, cachedLog, asm);



            //_controller.ReloadAll();

            var logWindow = CreateLogWindow(cachedLog);
            var mainWindow = CreateMainWindow(logWindow, resourceProvider);

            _view = WindowView.Create(mainWindow);
            _logView = WindowView.Create(logWindow);

            DontDestroyOnLoad(_view);
            DontDestroyOnLoad(_logView);


            var ab = FindObjectsOfType<AssetBase>();

            if (ab.Length == 0)
                _log.Error("Didn't find AssetBase");

            ab.Single().guiSkins.ToList().ForEach(skin => _log.Normal("GUISkin: " + skin.name));

            var fonts = Resources.FindObjectsOfTypeAll<Font>();

            if (fonts.Length == 0)
                _log.Error("Didn't find any fonts");

            fonts.ToList().ForEach(f => _log.Normal("font: " + f.name));

            var result = resourceProvider.GetTexture("AssemblyReloader.Resources.btnClose.png");
            if (!result.Any())
                _log.Error("Wasn't able to load from resource");
            else _log.Normal("Successfully loaded from resource");
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



        private IWindowComponent CreateMainWindow(IWindowComponent logWindowLogic, IResourceProvider resourceProvider)
        {
            var idProvider = new UniqueWindowIdProvider();


            //var style = new GUIStyle(AssetBase.GetGUISkin("KSP window 3").button);
            var style = new GUIStyle(HighLogic.Skin.button);

            var btnBackground = resourceProvider.GetTexture("AssemblyReloader.Resources.btnBackground.png");
            var btnClose = resourceProvider.GetTexture("AssemblyReloader.Resources.btnClose.png");

            if (btnBackground.Any() && btnClose.Any())
            {
                //style.active.background = style.normal.background = btnClose.First();
                style.hover.background = btnBackground.First();

                //style.active.background = style.normal.background = style.hover.background = tempTexture;//
            }
            else _log.Error("Failed to find MainWindow button textures");

            var logic = new MainViewLogic(_controller, logWindowLogic);

            var basicWindow = new BasicWindow(
                logic,
                new Rect(400f, 400f, 300f, 300f),
                idProvider.Get(), AssetBase.GetGUISkin("KSP window 6"),
                true,
                true
                ) {Title = "ART: Assembly Reloading Tool"};


            var tbButtons = new TitleBarButtons(basicWindow, new Vector2(4f, 4f));


            tbButtons.AddButton(new TitleBarButton(style, btnClose.First(), s => { }, "Test"));


            var hiding = new HideOnF2(tbButtons);

            return hiding;
        }



        private IWindowComponent CreateLogWindow(ICachedLog cachedLog)
        {
            var logWindowLogic = new LogViewLogic(cachedLog);
            var idProvider = new UniqueWindowIdProvider();

            var logWindow = new BasicWindow(logWindowLogic, new Rect(400f, 0f, 200f, 128f), idProvider.Get(),
                HighLogic.Skin, true, true) {Title = "ART Log", Visible = false};


            return logWindow;
        }
    }
}
