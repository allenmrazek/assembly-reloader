using System;

namespace AssemblyReloader.Queries
{
    class StartupSceneFromGameSceneQuery
    {
        public KSPAddon.Startup Query(GameScenes scene)
        {
            switch (scene)
            {
                case GameScenes.CREDITS:
                    return KSPAddon.Startup.Credits;

                case GameScenes.EDITOR:
                    return KSPAddon.Startup.EditorAny;

                case GameScenes.FLIGHT:
                    return KSPAddon.Startup.Flight;
                    
                case GameScenes.LOADING:
                    return KSPAddon.Startup.Instantly;

                case GameScenes.LOADINGBUFFER:
                    return KSPAddon.Startup.Instantly;

                case GameScenes.MAINMENU:
                    return KSPAddon.Startup.MainMenu;

                case GameScenes.PSYSTEM:
                    return KSPAddon.Startup.PSystemSpawn;

                case GameScenes.SETTINGS:
                    return KSPAddon.Startup.Settings;

                case GameScenes.SPACECENTER:
                    return KSPAddon.Startup.SpaceCentre;

                case GameScenes.TRACKSTATION:
                    return KSPAddon.Startup.TrackingStation;

                default:
                    throw new NotImplementedException(scene.ToString() + " not implemented");
            }
        }


    }
}
