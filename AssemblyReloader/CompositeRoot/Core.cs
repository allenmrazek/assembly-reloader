using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using AssemblyReloader.AssemblyTracking;
using AssemblyReloader.AssemblyTracking.Implementations;
using AssemblyReloader.Events;
using AssemblyReloader.Events.Implementations;
using AssemblyReloader.GUI;
using AssemblyReloader.GUI.Window.Factories;
using AssemblyReloader.Loaders.Addon.Factories.Implementations;
using AssemblyReloader.Loaders.Factories.Implementations;
using AssemblyReloader.Logging;
using AssemblyReloader.Logging.Implementations;
using AssemblyReloader.Mediators.Implementations;
using AssemblyReloader.Messages;
using AssemblyReloader.Providers;
using AssemblyReloader.Providers.Implementations;
using AssemblyReloader.Queries;
using AssemblyReloader.Queries.Implementations;
using ReeperCommon.FileSystem;
using ReeperCommon.FileSystem.Factories;
using ReeperCommon.FileSystem.Implementations;
using ReeperCommon.FileSystem.Implementations.Providers;
using ReeperCommon.Gui.Window;
using ReeperCommon.Gui.Window.Buttons;
using ReeperCommon.Gui.Window.Decorators;
using ReeperCommon.Gui.Window.Providers;
using ReeperCommon.Gui.Window.View;
using ReeperCommon.Logging;
using ReeperCommon.Logging.Implementations;
using ReeperCommon.Repositories.Resources;
using ReeperCommon.Repositories.Resources.Implementations;
using ReeperCommon.Repositories.Resources.Implementations.Decorators;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AssemblyReloader.CompositeRoot
{
    // composite root
    class Core
    {
        private readonly WindowView _view;
        private readonly WindowView _logView;

        private readonly IReloadableController _controller;
        private readonly MessageChannel _messageChannel;
        private readonly ICurrentGameSceneQuery _currentSceneQuery;
        private readonly IGameEventSubscriber<GameScenes> _eventOnLevelWasLoaded;

        private readonly ILog _log;



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




        public Core()
        {

#if DEBUG
            var primaryLog = new DebugLog("ART");
#else
            var primaryLog = LogFactory.Create(LogLevel.Standard);
#endif

            var cachedLog = new CachedLog(primaryLog, 100);

            _log = cachedLog;


            var fsFactory = new KSPFileSystemFactory(
                new KSPUrlDir(new KSPGameDataUrlDirProvider().Get()));

            var ourDirProvider = new AssemblyDirectoryQuery(Assembly.GetExecutingAssembly(), fsFactory.GetGameDataDirectory(), _log);
                

            var queryProvider = new QueryFactory();
            _currentSceneQuery = queryProvider.GetCurrentGameSceneProvider();





            var resourceLocator = ConfigureResourceRepository(ourDirProvider.Get());


            
            var destructionMediator = new GameObjectDestroyForReload();
            var addonFactory = new AddonFactory(destructionMediator, cachedLog.CreateTag("AddonFactory"));

            var infoFactory = new AddonInfoFactory(queryProvider.GetAddonAttributeQuery());

            _eventOnLevelWasLoaded = new GameEventSubscriber<GameScenes>(cachedLog.CreateTag("OnLevelWasLoaded"));
                // do not subscribe to GameEvents version of this event because it can be invoked twice in succession due to a KSP bug
                // instead, it is invoked by Core.OnLevelWasLoaded

            var loaderFactory = new LoaderFactory(addonFactory, infoFactory, cachedLog, queryProvider);









            _log.Normal("Initializing container...");

            var reloadableAssemblyFileQuery = new ReloadableAssemblyFilesInDirectoryQuery(fsFactory.GetGameDataDirectory());

            var reloadables = reloadableAssemblyFileQuery.Get().Select(raFile =>
                new ReloadableAssembly(new ReloadableReloadableIdentity(raFile),
                loaderFactory,
                _eventOnLevelWasLoaded,
                cachedLog.CreateTag("Reloadable:" + raFile.Name),
                queryProvider)).Cast<IReloadableAssembly>().ToList();


            reloadables.ForEach(r =>
            {
                r.Load();
                r.StartAddons(KSPAddon.Startup.Instantly);
            });

            _controller = new ReloadableController(queryProvider, cachedLog, reloadables);

            var logWindowFactory = new LogWindowFactory(cachedLog);
            var mainWindowFactory = new MainWindowFactory(resourceLocator);

            var logWindow = logWindowFactory.Create();
            var mainWindow = mainWindowFactory.Create(new MainViewWindowLogic(_controller, logWindow));

            _view = WindowView.Create(mainWindow);
            _logView = WindowView.Create(logWindow);

            Object.DontDestroyOnLoad(_view);
            Object.DontDestroyOnLoad(_logView);
        }





        // Unity MonoBehaviour callback
        // ReSharper disable once UnusedMember.Local
        public void LevelWasLoaded(int level)
        {
            _eventOnLevelWasLoaded.OnEvent(_currentSceneQuery.Get());
        }



        //private IWindowComponent CreateMainWindow(
        //    IWindowComponent logWindowLogic, 
        //    IResourceRepository resourceProvider,
        //    IFileSystemFactory fsFactory)
        //{
        //    var idProvider = new UniqueWindowIdProvider();


        //    //var style = new GUIStyle(AssetBase.GetGUISkin("KSP window 3").button);
        //    var style = new GUIStyle(HighLogic.Skin.button) {border = new RectOffset(), padding = new RectOffset()};
        //    style.fixedHeight = style.fixedWidth = 16f;
        //    style.margin = new RectOffset();

        //    var btnClose = resourceProvider.GetTexture("Resources/btnClose.png");


        //    var logic = new MainViewLogic(_controller, logWindowLogic);

        //    var basicWindow = new BasicWindow(
        //        logic,
        //        new Rect(400f, 400f, 300f, 300f),
        //        idProvider.Get(), HighLogic.Skin /*AssetBase.GetGUISkin("KSP window 6")*/
  
                
        //        ) { Title = "ART: Assembly Reloading Tool" };


        //    var tbButtons = new TitleBarButtons(basicWindow, TitleBarButtons.ButtonAlignment.Right, new Vector2(3f, 3f));

        //    var tex = new Texture2D(1, 1);
        //    tex.LoadImage(
        //        System.IO.File.ReadAllBytes(
        //            (fsFactory.GetGameDataDirectory()).File(
        //                "AssemblyReloader/Resources/btnClose.png").Single().FullPath));

        //    tbButtons.AddButton(new TitleBarButton(style, btnClose.First(), s => { }, "Test"));
        //    tbButtons.AddButton(new TitleBarButton(style, btnClose.First(), s => { }, "Test2"));
        //    tbButtons.AddButton(new TitleBarButton(style, btnClose.First(), s => { }, "Test3"));
        //    tbButtons.AddButton(new TitleBarButton(style, btnClose.First(), s => { }, "Tefsdst2"));
        //    tbButtons.AddButton(new TitleBarButton(style, btnClose.First(), s => { }, "Tefsdfdst2"));
        //    tbButtons.AddButton(new TitleBarButton(style, btnClose.First(), s => { }, "Tefddsffsdst2"));

        //    var hiding = new HideOnF2(tbButtons);

        //    var clamp = new ClampToScreen(hiding);

        //    return clamp;
        //}



        //private IWindowComponent CreateLogWindow(ICachedLog cachedLog)
        //{
        //    var logWindowLogic = new LogViewLogic(cachedLog);
        //    var idProvider = new UniqueWindowIdProvider();

        //    var logWindow = new BasicWindow(logWindowLogic, new Rect(400f, 0f, 200f, 128f), idProvider.Get(),
        //        HighLogic.Skin, true) {Title = "ART Log", Visible = false};


        //    return logWindow;
        //}



        private IResourceRepository ConfigureResourceRepository(IDirectory dllDirectory)
        {
            return new ResourceRepositoryComposite(
                // search GameDatabase first
                //   note: GameDatabase expects extensionless urls
                    new ResourceIdentifierAdapter(id =>
                    {
                        if (!Path.HasExtension(id) || string.IsNullOrEmpty(id)) return id;

                        var dir = Path.GetDirectoryName(id) ?? "";
                        var woExt = Path.Combine(dir, Path.GetFileNameWithoutExtension(id)).Replace('\\', '/');

                        return !string.IsNullOrEmpty(woExt) ? woExt : id;
                    }, new ResourceFromGameDatabase()
                    ),

                // then look at physical file system. These work on a list of items cached
                // by GameDatabase rather than working directly with the disk (unless a resource 
                // is accessed from here, of course)
                    new ResourceFromDirectory(dllDirectory),


                // finally search embedded resource
                //   note: embedded resource ids should be appended by assembly namespace
                    new ResourceIdentifierAdapter(id => Assembly.GetExecutingAssembly().GetName().Name + "." + id,

                //   note: expects resource manifest ids (periods instead of slashes), so 
                //         identifier transformer is needed if we want to specify input identifiers
                //         as though they were paths
                    new ResourceIdentifierAdapter(id => id.Replace('/', '.').Replace('\\', '.'),
                        new ResourceFromEmbeddedResource(Assembly.GetExecutingAssembly()))
                    ));
        }
    }
}
