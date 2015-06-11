﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using ReeperCommon.Containers;
using ReeperCommon.Extensions;
using ReeperCommon.Logging;
using UnityEngine;
using Object = UnityEngine.Object;


namespace TestProject
{

//[KSPScenario(ScenarioCreationOptions.AddToAllGames, new [] { GameScenes.SPACECENTER })]
//public class FacilityLevelTweaker : ScenarioModule
//{
//    private IEnumerator Start()
//    {
//        yield return 0;

//        var target = ScenarioUpgradeableFacilities.protoUpgradeables.Single(pu => pu.Key == "SpaceCenter/TrackingStation");
//        target.Value.facilityRefs.ForEach(fr => fr.SetLevel(1));
//    }
//}

    //[KSPAddon(KSPAddon.Startup.Instantly, true)]
    //public class Installer : MonoBehaviour
    //{
    //    private void Start()
    //    {
    //        DontDestroyOnLoad(this);
            
    //            GetFairingParts().ToList().ForEach(ap =>
    //            {
    //                var instance = ap.partPrefab.gameObject.AddComponent<NoiseMaker>();

    //                typeof (PartModule).GetMethod("Awake", BindingFlags.Instance | BindingFlags.NonPublic)
    //                    .Invoke(instance, null);
    //            });
    //    }

    //    private void OnPluginReloadRequested()
    //    {
    //        print("Reload requested -- shutting down");
    //        GetFairingParts().ToList().ForEach(fp => Destroy(fp.partPrefab.gameObject.GetComponent<NoiseMaker>()));
    //    }


    //    private static IEnumerable<AvailablePart> GetFairingParts()
    //    {
    //        return PartLoader.LoadedPartsList.Where(ap => ap.partPrefab.GetComponent<ModuleProceduralFairing>() != null);
    //    }
    //}

    //[KSPAddon(KSPAddon.Startup.Flight, false)]
    //public class ConfigDumper : MonoBehaviour
    //{
    //    private IEnumerator Start()
    //    {
    //        yield return new WaitForSeconds(5f);

    //        print(ShipConstruction.ShipConfig.ToString());
    //    }
    //}

    //public class NoiseMaker : PartModule
    //{
    //    private readonly ILog _log = new DebugLog("NoiseMaker");

    //    public override void OnAwake()
    //    {
    //        _log.Normal("OnAwake");
    //        base.OnAwake();
    //    }

    //    public override void OnStart(StartState state)
    //    {
    //        _log.Normal("OnStart");
    //        base.OnStart(state);
    //    }

    //    private void Start()
    //    {
    //        _log.Normal("Start");

    //    }
    //    public override void OnLoad(ConfigNode node)
    //    {
    //        _log.Normal("OnLoad");
    //        base.OnLoad(node);
    //    }

    //    public override void OnSave(ConfigNode node)
    //    {
    //        _log.Normal("OnSave");
    //        base.OnSave(node);
    //    }
    //}
    //public class ModuleProceduralFairingTweaker : PartModule
    //{
    //    /*
    //     * Current Functionality:
    //     * - Make number of pieces per section (nArcs) tweakable in-game
    //     * - Make section height (xSectionHeightMax) tweakable in-game
    //     * - Make edge smoothing (edgeWarp) tweakable in-game
    //     */

    //    [UI_FloatRange(minValue = 2.0f, maxValue = 6.0f, stepIncrement = 1.0f)]
    //    [KSPField(guiName = "Divisions", guiActiveEditor = true)]
    //    float tweakArcs = 2.0f;

    //    [UI_FloatRange(minValue = 2.0f, maxValue = 6.0f, stepIncrement = 0.1f)]
    //    [KSPField(guiName = "Max Height", guiActiveEditor = true)]
    //    float tweakSectionHeightMax = 2.0f;

    //    [UI_FloatRange(minValue = 0.00f, maxValue = 0.10f, stepIncrement = 0.01f)]
    //    [KSPField(guiName = "Edge Smooth", guiActiveEditor = true)]
    //    float tweakEdgeWarp = 0.00f;

    //    ModuleProceduralFairing pf;
    //    private readonly ILog _log = new DebugLog("FairingTweaker");

    //    private void Start()
    //    {
    //        _log.Normal("Start");
    //    }

    //    public override void OnAwake()
    //    {
    //        _log.Normal("OnAwake");
    //        base.OnAwake();

    //        pf = FindProceduralFairingModule();

    //        if (pf != null)
    //        {
    //            _log.Normal("Setting persistent stuff");

    //            //set ProceduralFairingModule's tweakable fields to persistant
    //            pf.Fields["nArcs"].isPersistant = true;
    //            pf.Fields["xSectionHeightMax"].isPersistant = true;
    //            pf.Fields["edgeWarp"].isPersistant = true;


    //        }
    //        else
    //        {
    //            PDebug.Log("StockFairingTweaker: ModuleProceduralFairing instance not found!");
    //            Fields["tweakArcs"].guiActiveEditor = false;
    //            Fields["tweakSectionHeightMax"].guiActiveEditor = false;
    //            Fields["tweakEdgeWarp"].guiActiveEditor = false;
    //        }
    //    }

    //    public override void OnStart(StartState state)
    //    {
    //        _log.Normal("OnStart");
    //        base.OnStart(state);


    //    }

    //    public override void OnLoad(ConfigNode node)
    //    {
    //        _log.Normal("OnLoad");
    //        base.OnLoad(node);


    //        //initial tweak bar settings
    //        tweakArcs = pf.nArcs;
    //        tweakSectionHeightMax = pf.xSectionHeightMax;
    //        tweakEdgeWarp = pf.edgeWarp;
    //    }

    //    public void Update()
    //    {
    //        if (pf != null && HighLogic.LoadedSceneIsEditor)
    //        {
    //            pf.nArcs = (int)tweakArcs;
    //            pf.xSectionHeightMax = tweakSectionHeightMax;
    //            pf.edgeWarp = tweakEdgeWarp;
    //        }
    //    }

    //    private ModuleProceduralFairing FindProceduralFairingModule()
    //    {
    //        foreach (PartModule module in part.Modules)
    //        {
    //            if (module.moduleName == "ModuleProceduralFairing") return (ModuleProceduralFairing)module;
    //        }

    //        return null;
    //    }
    //}

    //namespace StockFairingTweaker
    //{
    //    public class ModuleProceduralFairingTweaker : PartModule
    //    {
    //        /*
    //         * Current Functionality:
    //         * - Make number of pieces per section (nArcs) tweakable in-game
    //         * - Make section height (xSectionHeightMax) tweakable in-game
    //         * - Make edge smoothing (edgeWarp) tweakable in-game
    //         */

    //        [UI_FloatRange(minValue = 2.0f, maxValue = 6.0f, stepIncrement = 1.0f)]
    //        [KSPField(guiName = "Divisions", guiActiveEditor = true, isPersistant = true)]
    //        float tweakArcs = 2.0f;

    //        [UI_FloatRange(minValue = 2.0f, maxValue = 6.0f, stepIncrement = 0.1f)]
    //        [KSPField(guiName = "Max Height", guiActiveEditor = true, isPersistant = true)]
    //        float tweakSectionHeightMax = 2.0f;

    //        [UI_FloatRange(minValue = 0.00f, maxValue = 0.10f, stepIncrement = 0.01f)]
    //        [KSPField(guiName = "Edge Smooth", guiActiveEditor = true, isPersistant = true)]
    //        float tweakEdgeWarp = 0.00f;

    //        ModuleProceduralFairing pf;


    //        public void Update()
    //        {
    //            pf.nArcs = (int)tweakArcs;
    //            pf.xSectionHeightMax = tweakSectionHeightMax;
    //            pf.edgeWarp = tweakEdgeWarp;
    //        }


    //        public override void OnLoad(ConfigNode node)
    //        {
    //            base.OnLoad(node);
    //            pf = GetComponent<ModuleProceduralFairing>();
    //            enabled = HighLogic.LoadedSceneIsEditor && pf != null;

    //            if (pf != null)
    //            {
    //                pf.nArcs = (int)tweakArcs;
    //                pf.xSectionHeightMax = tweakSectionHeightMax;
    //                pf.edgeWarp = tweakEdgeWarp;
    //            }
    //            else
    //            {
    //                PDebug.Log("StockFairingTweaker: ModuleProceduralFairing instance not found!");
    //                Fields["tweakArcs"].guiActiveEditor = false;
    //                Fields["tweakSectionHeightMax"].guiActiveEditor = false;
    //                Fields["tweakEdgeWarp"].guiActiveEditor = false;
    //            }
    //        }
    //    }
    //}


//    //[KSPAddon(KSPAddon.Startup.SpaceCentre, true)]
//    public class MakePersistent : MonoBehaviour
//    {
//        private void Start()
//        {
//            PartLoader.LoadedPartsList.Where(ap => ap.partPrefab.GetComponent<ModuleProceduralFairing>() != null)
//                .Select(ap => ap.partPrefab.GetComponent<ModuleProceduralFairing>())
//                .ToList()
//                .ForEach(pf => pf.Fields["nArcs"].isPersistant = true);
//        }
//    }

//    [KSPAddon(KSPAddon.Startup.Flight, false)]
//    public class PressButtonForConfig : MonoBehaviour
//    {
//        private void Update()
//        {
//            if (Input.GetKeyDown(KeyCode.Keypad5))
//            {
//                print("Getting ship config now...");
//                print(ShipConstruction.ShipConfig.ToString());
//            }
//        }
//    }

//        /*
// * Author: Chase Barnes (Wheffle)
// * <altoid287@gmail.com>
// * 19 May 2015
// */


//    namespace StockFairingTweaker
//    {
//        public class ModuleProceduralFairingTweaker : PartModule
//        {
//            /*
//             * Current Functionality:
//             * - Make number of pieces per section (nArcs) tweakable in-game
//             * - Make section height (xSectionHeightMax) tweakable in-game
//             * - Make edge smoothing (edgeWarp) tweakable in-game
//             */

//            [UI_FloatRange(minValue = 2.0f, maxValue = 6.0f, stepIncrement = 1.0f)]
//            [KSPField(guiName = "Divisions", guiActiveEditor = true)]
//            float tweakArcs = 2.0f;

//            [UI_FloatRange(minValue = 2.0f, maxValue = 6.0f, stepIncrement = 0.1f)]
//            [KSPField(guiName = "Max Height", guiActiveEditor = true)]
//            float tweakSectionHeightMax = 2.0f;

//            [UI_FloatRange(minValue = 0.00f, maxValue = 0.10f, stepIncrement = 0.01f)]
//            [KSPField(guiName = "Edge Smooth", guiActiveEditor = true)]
//            float tweakEdgeWarp = 0.00f;

//            ModuleProceduralFairing pf;

//            public override void OnStart(StartState state)
//            {
//                base.OnStart(state);

//                //Debug.Log("PFairingTweaker.OnStart");

//                //pf = FindProceduralFairingModule();

//                //if (pf != null)
//                //{
//                //    //set ProceduralFairingModule's tweakable fields to persistant
//                //    //pf.Fields["nArcs"].isPersistant = true;
//                //    //pf.Fields["xSectionHeightMax"].isPersistant = true;
//                //    //pf.Fields["edgeWarp"].isPersistant = true;

//                //    //initial tweak bar settings
//                //    tweakArcs = pf.nArcs;
//                //    tweakSectionHeightMax = pf.xSectionHeightMax;
//                //    tweakEdgeWarp = pf.edgeWarp;

//                //}
//                //else
//                //{
//                //    Debug.Log("StockFairingTweaker: ModuleProceduralFairing instance not found!");
//                //    Fields["tweakArcs"].guiActiveEditor = false;
//                //    Fields["tweakSectionHeightMax"].guiActiveEditor = false;
//                //    Fields["tweakEdgeWarp"].guiActiveEditor = false;
//                //}
//            }

//            public void Update()
//            {
//                if (pf != null && HighLogic.LoadedSceneIsEditor)
//                {
//                    pf.nArcs = (int)tweakArcs;
//                    pf.xSectionHeightMax = tweakSectionHeightMax;
//                    pf.edgeWarp = tweakEdgeWarp;
//                }
//            }

//            private ModuleProceduralFairing FindProceduralFairingModule()
//            {
//                foreach (PartModule module in part.Modules)
//                {
//                    if (module.moduleName == "ModuleProceduralFairing") 
//                    {
//                        return (ModuleProceduralFairing) module;
//                    }
//                }

//                Debug.LogWarning("Did not find fairing module");

//                return null;
//            }

//            public override void OnLoad(ConfigNode node)
//            {
//                base.OnLoad(node);
//                Debug.Log("PFairingTweaker.OnLoad");
//            }

//            public override void OnSave(ConfigNode node)
//            {
//                base.OnSave(node);
//                Debug.Log("PFairingTweaker.OnSave");
//            }

//            public override void OnAwake()
//            {
//                base.OnActive();
//                Debug.Log("PFairingTweaker.OnAwake");


//                pf = FindProceduralFairingModule();

//                if (pf != null)
//                {
//                    Debug.Log("nArcs persistent: " + pf.Fields["nArcs"].isPersistant);

//                    //set ProceduralFairingModule's tweakable fields to persistant
//                    pf.Fields["nArcs"].isPersistant = true;
//                    pf.Fields["xSectionHeightMax"].isPersistant = true;
//                    pf.Fields["edgeWarp"].isPersistant = true;

//                    //initial tweak bar settings
//                    tweakArcs = pf.nArcs;
//                    tweakSectionHeightMax = pf.xSectionHeightMax;
//                    tweakEdgeWarp = pf.edgeWarp;

//                }
//                else
//                {
//                    Debug.Log("StockFairingTweaker: ModuleProceduralFairing instance not found!");
//                    Fields["tweakArcs"].guiActiveEditor = false;
//                    Fields["tweakSectionHeightMax"].guiActiveEditor = false;
//                    Fields["tweakEdgeWarp"].guiActiveEditor = false;
//                }
//            }


//        }
//    }



////[KSPAddon(KSPAddon.Startup.MainMenu, true)]
//public class PartModelPreviewer : MonoBehaviour
//{
//    private const int PreviewWidth = 200;
//    private const int PreviewHeight = 200;
//    private const int RenderLayer = 8;
//    private const float PreviewTime = 2f;
//    private const float PreviewRevolutionRate = 180f;
//    private const float TargetPreviewSize = 0.7f; // size 1 would take up the whole vertical camera space

//    private string _current = "<None>";

//    private void Start()
//    {
//        gameObject.AddComponent<Camera>();

//        camera.cullingMask = (1 << RenderLayer);
//        camera.clearFlags = ~CameraClearFlags.Nothing;
//        camera.nearClipPlane = 0.1f;
//        camera.farClipPlane = 10f;
//        camera.orthographic = true;
//        camera.backgroundColor = new Color(0f, .7f, 0f, 0f);

//        camera.orthographicSize = 0.5f;
//        camera.pixelRect = new Rect(0f, Screen.height - PreviewHeight, PreviewWidth, PreviewHeight);

//        transform.position = new Vector3(0f, 0f, -8f);
//        transform.LookAt(Vector3.zero, Vector3.up);

//        StartCoroutine(Cycle());
//    }


//    private IEnumerator Cycle()
//    {
//        while (true)
//        {
//            foreach (var part in PartLoader.LoadedPartsList.Where(ap => ap.iconPrefab != null))
//            {
//                _current = part.name;

//                var go = (GameObject) Instantiate(part.iconPrefab);
//                PartLoader.StripComponent<Collider>(go);
//                go.SetLayerRecursive(RenderLayer);

                    
//                go.SetActive(true);

//                var bounds = go.GetRendererBounds();
//                float multiplier = TargetPreviewSize / Mathf.Max(bounds.size.x, bounds.size.y, bounds.size.z); 
                    
//                go.transform.localScale = Vector3.one*multiplier;
//                go.transform.rotation = Quaternion.AngleAxis(-30f, Vector3.right);

//                transform.position = new Vector3(transform.position.x, bounds.center.y, transform.position.z);

//                go.transform.parent = transform;


//                yield return StartCoroutine(Preview(go));

//                Destroy(go);
//            }
//        }
//    }


//    private IEnumerator Preview(GameObject previewObject)
//    {
//        float start = Time.time;

//        while (Time.time - start < PreviewTime)
//        {
//            yield return 0;

//            previewObject.transform.Rotate(Vector3.up, Time.deltaTime * PreviewRevolutionRate);
//        }
//    }


//    private void OnGUI()
//    {
//        GUI.Label(new Rect(0, 0, 200f, 20f), "Displaying " + _current);
//    }
//}




//    [Serializable]
//    public class ExamplePersistentObject : IConfigNode, IPersistenceLoad
//    {
//        private readonly ILog _log = new DebugLog("ExampleObject");

//        [Persistent, KSPField (isPersistant = true)] public string MyString = "TestValue";
//        public void Load(ConfigNode node)
//        {
//            _log.Warning("Load, string is " + MyString);
//        }

//        public void Save(ConfigNode node)
//        {
//            _log.Warning("Save");
//        }

//        public void PersistenceLoad()
//        {
//            _log.Error("PersistenceLoad");
//        }
//    }

    //public class PersistentPartModule : PartModule
    //{
    //    private readonly ILog _log = new DebugLog("PersistentPartModule");

    //    [KSPField (isPersistant = true), Persistent] public ExamplePersistentObject Example = new ExamplePersistentObject();

    //    public override void OnAwake()
    //    {
    //        base.OnAwake();
    //        _log.Warning("OnAwake");
    //    }

    //    public override void OnLoad(ConfigNode node)
    //    {
    //        if (part == null || part.partInfo == null || part.partInfo.partPrefab == null || ReferenceEquals(part.partInfo.partPrefab.gameObject, gameObject))
    //        {
    //            _log.Warning("OnLoad: {0}", node.ToString());

    //            _log.Warning("Created: {0}", ConfigNode.CreateConfigFromObject(this).ToString());
    //            Example.MyString = "Modified by prefab";
    //        }
    //        else
    //        {
    //            _log.Error("Loaded from a prefab: Example.MyString is " + Example.MyString);
    //        }
    //        base.OnLoad(node);
    //    }

    //    public override void OnSave(ConfigNode node)
    //    {
    //        _log.Warning("OnSave");
    //        base.OnSave(node);
    //    }

    //    public override void OnStart(StartState state)
    //    {
    //        _log.Warning("OnStart with " + state);
    //        _log.Error("Loaded from a prefab: Example.MyString is " + Example.MyString);
    //        base.OnStart(state);
    //    }
    //}


    //[KSPAddon(KSPAddon.Startup.MainMenu, true)]
    //public class ExaminePersistence : MonoBehaviour
    //{
    //    private IEnumerator Start()
    //    {
    //        yield return new WaitForSeconds(3f);

    //        var pm =
    //            PartLoader.LoadedPartsList.Find(ap => ap.name == "mk1pod")
    //                .partPrefab.gameObject.GetComponent<PersistentPartModule>();

    //        if (pm == null) print("ERROR: couldn't find persistent part module");

    //        print("pm.Example = " + pm.Example.MyString);
    //    }
    //}




























    //[KSPAddon(KSPAddon.Startup.MainMenu, true)]
    //public class ConfigNodeTester : MonoBehaviour
    //{
    //    private void Start()
    //    {
    //        var cfg = new ConfigNode();

    //        cfg.AddNode("TEST_NODE").AddNode("TEST_NODE");

    //        print(cfg.ToString());

    //        print("Node count: " + cfg.nodes.Count);
    //    }
    //}


    ////[KSPAddon(KSPAddon.Startup.Flight, false)]
    //public class BiomeReporter : MonoBehaviour
    //{
    //    private Rect _windowRect = new Rect(0, 0, 200f, 30f);

    //    private void Start()
    //    {
    //        print("Dimensions: " + FlightGlobals.currentMainBody.BiomeMap.Width + "," +
    //              FlightGlobals.currentMainBody.BiomeMap.Height);
    //    }
    //    private void OnGUI()
    //    {
    //        _windowRect = GUILayout.Window(GetInstanceID(), _windowRect, DrawWindow, "Biome reporter");
    //    }

    //    private void DrawWindow(int winid)
    //    {
    //        var v = FlightGlobals.ActiveVessel;
    //        if (v == null) return;

    //        GUILayout.Label("Biome: " + ScienceUtil.GetExperimentBiome(v.mainBody, v.latitude, v.longitude));
    //        GUI.DragWindow();
    //    }
    //}

//[KSPAddon(KSPAddon.Startup.SpaceCentre, false)]
//public class AstronautComplexSpaceCentre : MonoBehaviour
//{
//    private void Start() { gameObject.AddComponent<ReprogramAstronautComplex>(); }
//}


//[KSPAddon(KSPAddon.Startup.EditorAny, false)]
//public class AstronautComplexEditor : AstronautComplexSpaceCentre
//{
//}


//public class ReprogramAstronautComplex : MonoBehaviour
//{
//    private readonly ILog _log = new DebugLog("Reprogrammer");
//    private Rect _windowRect = default(Rect);
//    private AstronautComplexApplicantPanel _applicantPanel;


//    private void Awake()
//    {
//        try
//        {
//            SetupSkin();

//            var complex = UIManager.instance.gameObject.GetComponentsInChildren<CMAstronautComplex>(true).FirstOrDefault();
//            if (complex == null) throw new Exception("Could not find astronaut complex");

//#if DEBUG
//            //DumpAssetBaseTextures();
//            //complex.gameObject.PrintComponents(new DebugLog("Complex"));
//            //UIManager.instance.gameObject.PrintComponents(new DebugLog("UIManager"));
//#endif

//            _applicantPanel = new AstronautComplexApplicantPanel(complex);

//            _windowRect = _applicantPanel.PanelArea;
//            _applicantPanel.Hide();

//            GameEvents.onGUIAstronautComplexSpawn.Add(AstronautComplexShown);
//            GameEvents.onGUIAstronautComplexDespawn.Add(AstronautComplexHidden);

//            enabled = false;
//        }
//        catch (Exception e)
//        {
//            _log.Error("Error: Encountered unhandled exception: " + e);
//            Destroy(this);
//        }

//    }

//    private void AstronautComplexShown()
//    {
//        enabled = true;
//    }

//    private void AstronautComplexHidden()
//    {
//        enabled = false;
//    }

//    private void OnDestroy()
//    {
//        GameEvents.onGUIAstronautComplexDespawn.Remove(AstronautComplexHidden);
//        GameEvents.onGUIAstronautComplexSpawn.Remove(AstronautComplexShown);
//    }

//    private void SetupSkin()
//    {
//        _customWindowSkin = new GUIStyle(HighLogic.Skin.window)
//        {
//            contentOffset = Vector2.zero,
//            padding = new RectOffset() { left = 0, right = HighLogic.Skin.window.padding.right, top = 0, bottom = 0 }
//        };
//    }


//    private void DumpAssetBaseTextures()
//    {
//        // note: there are apparently null entries in this list for some reason, hence the check
//        FindObjectOfType<AssetBase>().textures.Where(t => t != null).ToList().ForEach(t => _log.Debug("AssetBase: " + t.name));
//    }


//    private Vector2 _scroll = default(Vector2);
//    private GUIStyle _customWindowSkin;


//    private readonly Texture2D _portraitMale = AssetBase.GetTexture("kerbalicon_recruit");
//    private readonly Texture2D _portraitFemale = AssetBase.GetTexture("kerbalicon_recruit_female");

//    private void OnGUI()
//    {
//        GUI.skin = HighLogic.Skin;
//        var roster = HighLogic.CurrentGame.CrewRoster;

//        GUILayout.BeginArea(_windowRect, _customWindowSkin);
//        {
//            GUILayout.Label("Candidates");
//            _scroll = GUILayout.BeginScrollView(_scroll, _customWindowSkin);
//            {
//                foreach (var applicant in roster.Applicants)
//                    DrawListItem(applicant);
//            }
//            GUILayout.EndScrollView();
//        }
//        GUILayout.EndArea();
//    }


//    private void DrawListItem(ProtoCrewMember crew)
//    {
//        GUILayout.BeginVertical(_customWindowSkin, GUILayout.ExpandHeight(false));
//        {
//            GUILayout.BeginHorizontal(GUILayout.ExpandHeight(true), GUILayout.MaxHeight(_portraitMale.height),
//                GUILayout.ExpandWidth(true));
//            {
//                GUILayout.Label(crew.gender == ProtoCrewMember.Gender.Male ? _portraitMale : _portraitFemale);

//                GUILayout.BeginVertical(GUILayout.ExpandHeight(false), GUILayout.ExpandWidth(true));
//                {
//                    GUILayout.BeginHorizontal();
//                    GUILayout.Label(crew.name);
//                    GUILayout.FlexibleSpace();
//                    GUILayout.Label("Trained " + crew.experienceTrait.Title);
//                    GUILayout.EndHorizontal();

//                    GUILayout.Label("More info here");

//                    GUILayout.BeginHorizontal();
//                    GUILayout.FlexibleSpace();
//                    GUILayout.Button("Hire Applicant", GUILayout.MaxWidth(100f)); // todo: hire logic 
//                    GUILayout.EndHorizontal();
//                }
//                GUILayout.EndVertical();
//            }
//            GUILayout.EndHorizontal();
//        }
//        GUILayout.EndVertical();
//    }
//}


//public class AstronautComplexApplicantPanel
//{
//    private readonly Transform _applicantPanel;

//    public AstronautComplexApplicantPanel(CMAstronautComplex astronautComplex)
//    {
//        if (astronautComplex == null) throw new ArgumentNullException("astronautComplex");
//        _applicantPanel = astronautComplex.transform.Find("CrewPanels/panel_applicants");

//        if (_applicantPanel == null)
//            throw new ArgumentException("No applicant panel found on " + astronautComplex.name);
//    }

//    public void Hide(bool tf = true)
//    {
//        _applicantPanel.gameObject.SetActive(!tf);
//    }


//    public Rect PanelArea
//    {
//        get
//        {
//            // whole panel is an EzGUI BTButton and we'll be needing to know about its renderer to come up
//            // with screen coordinates
//            var button = _applicantPanel.GetComponent<BTButton>() as SpriteRoot;

//            if (button == null)
//                throw new Exception("AstronautComplexApplicantPanel: Couldn't find BTButton on " +
//                                    _applicantPanel.name);

//            var uiCam = UIManager.instance.uiCameras.FirstOrDefault(uic => (uic.mask & (1 << _applicantPanel.gameObject.layer)) != 0);
//            if (uiCam == null)
//                throw new Exception("AstronautComplexApplicantPanel: Couldn't find a UICamera for panel");

//            var screenPos = uiCam.camera.WorldToScreenPoint(_applicantPanel.position);

//            return new Rect(screenPos.x, Screen.height - screenPos.y, button.width, button.height);
//        }
//    }
//}


    //[KSPAddon(KSPAddon.Startup.MainMenu, true)]
    public class ModifyScienceWarningText : MonoBehaviour
    {
        private void Start()
        {
            PartLoader.LoadedPartsList
                .Where(ap => ap.partPrefab.GetComponent<ModuleScienceExperiment>() != null)
                .Select(ap => ap.partPrefab.GetComponent<ModuleScienceExperiment>())
                .ToList()
                .ForEach(mse => mse.collectWarningText = "Some warning text here");
        }
    }



    //[KSPAddon(KSPAddon.Startup.Flight, false)]
    public class ArrowDirTest : MonoBehaviour
    {
        private ArrowPointer _pointer;

        private void Start()
        {
            _pointer = ArrowPointer.Create(FlightGlobals.ActiveVessel.rootPart.transform, Vector3.zero, Vector3.up, 100, Color.red, true);
        }

        private void Update()
        {
            var dir = Vector3.ProjectOnPlane(FlightGlobals.ActiveVessel.srf_velocity, FlightGlobals.upAxis).normalized;
            _pointer.Direction = dir;

            print("Dir: " + dir);
        }

        private void OnDestroy()
        {
            Destroy(_pointer);
        }
    }

    //[KSPAddon(KSPAddon.Startup.Instantly, true)]
    //public class DumpAssemblyLocations : MonoBehaviour
    //{
    //    private void Start()
    //    {
    //        foreach (AssemblyLoader.LoadedAssembly mod in AssemblyLoader.loadedAssemblies)
    //            print("Location: " + mod.assembly.Location);
    //    }
    //}

    //[KSPAddon(KSPAddon.Startup.MainMenu, true)]
    //public class DumpEvaConfig : MonoBehaviour
    //{
    //    private void Start()
    //    {
    //        var eva = PartLoader.LoadedPartsList.FirstOrDefault(ap => ap.name == "kerbalEVA");

    //        if (eva == null)
    //        {
    //            print("ERROR: failed to find eva part");

    //            PartLoader.LoadedPartsList.ForEach(ap => print("AP: " + ap.name));
    //        }
    //        else
    //        {
    //            var config = GameDatabase.Instance.GetConfigs("PART").FirstOrDefault(u => u.name.Replace('_', '.') == "kerbalEVA");
    //            if (config == null) print("ERROR: couldn't find config");

    //            print(config.ToString());
    //        }
    //    }
    //}

    //[KSPAddon(KSPAddon.Startup.Instantly, true)]
    public class TestCoroutinesOnDisabled : MonoBehaviour
    {
        private void Awake()
        {
            enabled = false;
        }

        private IEnumerator Start()
        {
            print("Test begins");

            enabled = false;
            yield return StartCoroutine(WaitForTime(3f));
            enabled = true;

            print("Test complete2");
        }

        private IEnumerator WaitForTime(float tSeconds)
        {
            var startTime = Time.time;
            //enabled = false;

            print("Time: " + Time.time);
            print("Started at: " + startTime);

            while (Time.time - startTime < tSeconds)
            {
                //print("Waiting ...");
                yield return 0;
            }

            print("Wait is over!");
        }

        private void Update()
        {
            if (enabled)
            {
                //print("Update!");
            }
            else print("BADUPDATE!");
        }
    }
}