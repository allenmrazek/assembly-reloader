using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using AssemblyReloader.AssemblyTracking;
using AssemblyReloader.AssemblyTracking.Implementations;
using AssemblyReloader.GUI;
using AssemblyReloader.GUI.Window.Factories;
using AssemblyReloader.Loaders.Addon.Factories.Implementations;
using AssemblyReloader.Loaders.Factories.Implementations;
using AssemblyReloader.Logging.Implementations;
using AssemblyReloader.Mediators.Implementations;
using AssemblyReloader.Messages;
using AssemblyReloader.Queries;
using AssemblyReloader.Queries.Implementations;
using ReeperCommon.Events;
using ReeperCommon.Events.Implementations;
using ReeperCommon.FileSystem;
using ReeperCommon.FileSystem.Implementations;
using ReeperCommon.FileSystem.Implementations.Providers;
using ReeperCommon.Gui.Window.View;
using ReeperCommon.Logging;
using ReeperCommon.Logging.Factories;
using ReeperCommon.Logging.Implementations;
using ReeperCommon.Repositories.Resources;
using ReeperCommon.Repositories.Resources.Implementations;
using ReeperCommon.Repositories.Resources.Implementations.Decorators;
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
        private readonly EventSubscriber<GameScenes> _eventOnLevelWasLoaded;

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

            _eventOnLevelWasLoaded = new EventSubscriber<GameScenes>();
                GameEvents.onLevelWasLoaded.Add(_eventOnLevelWasLoaded.OnEvent);


                // do not subscribe to GameEvents version of this event because it can be invoked twice in succession due to a KSP bug
                // instead, it is invoked by Core.OnLevelWasLoaded

            var loaderFactory = new LoaderFactory(addonFactory, infoFactory, cachedLog, queryProvider);









            _log.Normal("Initializing container...");

            var reloadableAssemblyFileQuery = new ReloadableAssemblyFilesInDirectoryQuery(fsFactory.GetGameDataDirectory());

            var reloadables = reloadableAssemblyFileQuery.Get().Select(raFile =>
                new ReloadableAssembly(
                    raFile,
                loaderFactory,
                _eventOnLevelWasLoaded,
                cachedLog.CreateTag("Reloadable:" + raFile.Name),
                queryProvider)).Cast<IReloadableAssembly>().ToList();


            reloadables.ForEach(r =>
            {
                r.Load();
                r.StartAddons(KSPAddon.Startup.Instantly);
            });

            _controller = new ReloadableController(queryProvider, reloadables);

            var windowFactory = new WindowFactory(resourceLocator, cachedLog);

            var logWindow = windowFactory.CreateLogWindow();
            var mainWindow = windowFactory.CreateMainWindow(new MainViewWindowLogic(_controller, logWindow));

            _view = WindowView.Create(mainWindow);
            _logView = WindowView.Create(logWindow);

            Object.DontDestroyOnLoad(_view);
            Object.DontDestroyOnLoad(_logView);
        }







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
