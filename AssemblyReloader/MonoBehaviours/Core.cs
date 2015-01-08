using System.Linq;
using AssemblyReloader.AddonTracking;
using AssemblyReloader.Factory;
using AssemblyReloader.GUI;
using AssemblyReloader.Logging;
using AssemblyReloader.Providers;
using AssemblyReloader.Queries;
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


        private void Start()
        {
            DontDestroyOnLoad(this);

#if DEBUG
            var log = LogFactory.Create(LogLevel.Debug);
#else
            var log = LogFactory.Create(LogLevel.Standard);
#endif


            var gameDataProvider = new KSPGameDataDirectoryProvider();
            var startupSceneFromGameScene = new KspStartupSceneFromGameSceneQuery();

            var currentSceneProvider = new KspCurrentStartupSceneProvider(startupSceneFromGameScene);

            var reloadableAssemblyFactory = new Factory.ReloadableAssemblyFactory(new KspAddonsFromAssemblyQuery());

            var reloaderLog = new ReloaderLog(log, 25);

            var fileFactory = new KSPFileFactory();
            var loaderFactory = new LoaderFactory(reloaderLog, currentSceneProvider);

            var reloadableAssemblyFileQuery = new ReloadableAssemblyFileQuery(
                fileFactory,
                gameDataProvider
                );

            var infoFactory = new AddonInfoFactory(new KspAddonsFromAssemblyQuery());

            _container = new ReloadableContainer(
                                                    reloadableAssemblyFactory, 
                                                    loaderFactory,
                                                    infoFactory,
                                                    reloadableAssemblyFileQuery,
                                                    startupSceneFromGameScene,
                                                    reloaderLog
                                                );

            GameEvents.onLevelWasLoaded.Add(_container.HandleLevelLoad);
            
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
