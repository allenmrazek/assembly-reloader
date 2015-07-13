using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using ReeperCommon.Containers;
using ReeperCommon.Extensions;
using ReeperCommon.Logging;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;


namespace TestProject
{


    //[KSPAddon(KSPAddon.Startup.MainMenu, true)]
//    public class DumpPlanetTextures : MonoBehaviour
//    {
//        private void Start()
//        {
//            StartCoroutine(CreateTextures());
//            StartCoroutine(CreateKerbinMap("Duna"));
//            StartCoroutine(CreateKerbinMap("Kerbin"));
//        }

//        IEnumerator CreateKerbinMap(string name)
//        {
//            var planetBody = FindObjectsOfType<CelestialBody>().Single(cb => cb.name == name).scaledBody;
//            planetBody.PrintComponents(new DebugLog(name));

//            for (int i = 0; i < 360; i += 36)
//            {
                
//                var target =
//                    planetBody.renderer.sharedMaterial.mainTexture.As2D().CreateReadable(planetBody.renderer.sharedMaterial);

//                target.SaveToDisk(name + "_.png");
//                Destroy(target);

//                yield return 0;
//            }

//        }

//        IEnumerator CreateTextures()
//        {
//            var bodies = FindObjectsOfType<CelestialBody>();

//            print("Found: " + bodies.Length + " CelestialBody");

            
//            foreach (var body in bodies.Where(cb => cb.name == "Kerbin" || cb.name == "Duna"))
//                //foreach (var r in body.GetComponentsInChildren<Renderer>(true))
//                foreach (var r in new [] { body.scaledBody.renderer })
//                {
//                    print("Color map for " + body.name);

//                    body.gameObject.PrintComponents(new DebugLog(body.name));

//                    if (r == null)
//                    {
//                        print("no r");
                        
//                        continue;
//                    }
//                    if (r.sharedMaterial == null)
//                    {
//                        print("no material");
//                        continue;
//                    }

//                    if (r.sharedMaterial.mainTexture == null)
//                    {
//                        print("<no main texture");
//                        continue;
//                    }
//                    if (r.sharedMaterial.mainTexture.As2D() == null)
//                        print("no conversion");

//                    print("Shader: " + r.sharedMaterial.shader.name);
//                    print("  with keywords: " + string.Join(", ", r.sharedMaterial.shaderKeywords));

//                    Shader.SetGlobalVector("_sunLightDirection", Vector3.back);

//                    var target = r.sharedMaterial.mainTexture.As2D().CreateReadable(r.sharedMaterial);
//                    if (target == null) print("failed to create readable");

//                    print("Creating texture of dimensions:" + target.width + "," + target.height);

//                    target.SaveToDisk("cb_" + body.name + "_" + r.name + "_COLR.png");

//                    Destroy(target);

//                    print("Bump map");
//                    if (r.sharedMaterial.GetTexture("_BumpMap") != null)
//                    {
//                        target = r.sharedMaterial.GetTexture("_BumpMap").As2D().CreateReadable();
//                        target.SaveToDisk("cb_" + body.name + "_" + r.name + "_BUMP.png");
//                        Destroy(target);
//                    }

//                    print("Spec Map");
//                    if (r.sharedMaterial.GetTexture("_SpecMap") != null)
//                    {
//                        target = r.sharedMaterial.GetTexture("_SpecMap").As2D().CreateReadable();
//                        target.SaveToDisk("cb_" + body.name + "_" + r.name + "_SPEC.png");
//                        Destroy(target);
//                    }

//                    yield return 0;
//                }
//        }
//    }


//    //[KSPAddon(KSPAddon.Startup.Flight, false)]
//    public class WasteMemoryRunner : MonoBehaviour
//    {
//        private const long KB = 1024;
//        private const long Megs = KB * 1024;

//        private Rect _rect = new Rect(200f, 200f, 200f, 200f);

//        [NonSerialized]
//        private System.Collections.IEnumerator _projector;

//        [NonSerialized]
//        private byte[] _wasted;

//        private Texture2D[] _wastedTextures;

//        //private List<byte> CreateByteList(int bytes)
//        //{
//        //    return new List<byte>(new byte[bytes]);
//        //}


//        private void Awake()
//        {
//            GameEvents.onGameSceneSwitchRequested.Add(Evt);

//        }

//        private void OnDestroy()
//        {
//            GameEvents.onGameSceneSwitchRequested.Remove(Evt);
//            //    Debug.LogWarning("MemoryWaster OnDestroy");
//            _wasted = null;
//            //    _projector = null;
//            //    GC.ReRegisterForFinalize(this);
//            GC.Collect(GC.MaxGeneration);
//        }

//        private void Evt(GameEvents.FromToAction<GameScenes, GameScenes> data)
//        {
//            Debug.LogWarning("Switching scenes");
//            Destroy(gameObject);
//        }


//        private void WasteTextures()
//        {
//            _wastedTextures = new Texture2D[250 / 12];

//            for (int i = 0; i < (250/12); ++i)
//            {
//                _wastedTextures[i] = new Texture2D(2048, 1024, TextureFormat.ARGB32, true);
//                var pixels = _wastedTextures[i].GetPixels32();
//                _wastedTextures[i].SetPixels32(pixels);
//                _wastedTextures[i].Apply();
//            }
//        }


//        private void Draw(int winid)
//        {
//            GUILayout.BeginVertical();
//            {
//                GUILayout.Label("Allocated: " + ((float)(GC.GetTotalMemory(false) / Megs)).ToString("F2"));
//                if (GUILayout.Button("Waste 10 megs")) WasteMemory(10 * Megs);
//                if (GUILayout.Button("Waste 50 megs")) WasteMemory(50 * Megs);
//                if (GUILayout.Button("Waste 250 megs")) WasteMemory(250 * Megs);
//                if (GUILayout.Button("Force Garbage Collection")) RunGarbageCollection();
//                GUILayout.Space(25f);
//                if (GUILayout.Button("Waste 250 without IEnumerator"))
//                {
//                    _wasted = new byte[250*Megs];
//                    //_wasted = CreateByteList((int)(250 * Megs));
//                }
//                if (GUILayout.Button("Free"))
//                    _wasted = null;
//                if (GUILayout.Button("Waste Textures")) WasteTextures();
//                if (GUILayout.Button("Free Textures"))
//                {
//// ReSharper disable once ForCanBeConvertedToForeach
//                    for (int i = 0; i < _wastedTextures.Length; ++i)
//                        Destroy(_wastedTextures[i]);
//                }
//            }
//            GUILayout.EndVertical();
//            GUI.DragWindow();
//        }


//        private void RunGarbageCollection()
//        {

//            GC.Collect();
//            GC.WaitForPendingFinalizers();
//            Resources.UnloadUnusedAssets();
//            GC.Collect();
//            GC.WaitForPendingFinalizers();
//            Resources.UnloadUnusedAssets();
//        }


//        private void WasteMemory(long bytes)
//        {

//            //_wasted = null;
//            //_wasted = new Byte[bytes];
//            _projector = DoWasteMemory(bytes);
//        }


//        private IEnumerator DoWasteMemory(long bytes)
//        {
//            Debug.Log("Wasting " + (bytes / Megs) + " MBs");
//            var start = Time.realtimeSinceStartup;

//            _wasted = new byte[bytes];
//            //_wasted = CreateByteList((int)(250 * bytes));

//            while (Time.realtimeSinceStartup - start < 3f)
//                yield return 0; // waste 3 seconds

//            ScreenMessages.PostScreenMessage("Wasted " + ((float)(bytes / Megs)).ToString("F2") + " MBs");
//            _projector = null;

//        }


//        private void OnGUI()
//        {
//            _rect = GUILayout.Window(5553, _rect, Draw, "Memory Waster 1.0");
//        }


//        private void Update()
//        {
//            if (_projector != null)
//                _projector.MoveNext();

//            //if (_projector != null)
//            //    if (!_projector.MoveNext())
//            //        _projector = null;
//        }




//        //~WasteMemoryRunner()
//        //{
//        //    Debug.LogWarning("WasteMemoryRunner finalizer");
//        //}
//    }
    //[KSPAddon(KSPAddon.Startup.SpaceCentre, false)]
    //public class CheckConfigNode : MonoBehaviour
    //{
    //    private void Awake()
    //    {
    //        GameEvents.onGameStateSaved.Add(OnGameSaved);
    //    }

    //    private void OnDestroy()
    //    {
    //        GameEvents.onGameStateSaved.Remove(OnGameSaved);
    //    }

    //    private void OnGameSaved(Game game)
    //    {
    //        print("Current contents of game.config:");

    //        print(game.config.ToString());
    //    }
    //}

    //public class ModuleCameraShot : VesselModule
    //{
    //    private bool takeHiResShot = false;
    //    private int resWidth = 128;
    //    private int resHeight = 128;


    //    public static string ScreenShotName(int width, int height)
    //    {
    //        return string.Format("{0}/screen_{1}x{2}_{3}.png",
    //                             Application.dataPath,
    //                             width, height,
    //                             System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
    //    }


    //    public void TakeHiResShot()
    //    {
    //        takeHiResShot = true;
    //    }


    //    //[KSPEvent(active=true,guiActive=true,guiName="Take Shot",name="Take Shot")]
    //    public void Update()
    //    {
    //        takeHiResShot |= Input.GetKeyDown("k");
    //        if (takeHiResShot)
    //        {
    //            float start = Time.realtimeSinceStartup;
    //            var stopwatch = Stopwatch.StartNew();

    //            var _vessel = GetComponent<Vessel>();
    //            var _cameraObject = new GameObject("ColourCam");
    //            _cameraObject.transform.position = _vessel.transform.position;
    //            _cameraObject.transform.LookAt(_vessel.mainBody.transform.position);
    //            _cameraObject.transform.Translate(new Vector3(0, 0, -10));
    //            //Debug.LogError("created camera");
    //            var _camera = _cameraObject.AddComponent<Camera>();
    //            RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
    //            _camera.targetTexture = rt;
    //            Texture2D groundShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
    //            _camera.Render();
    //            //Debug.LogError("rendered something...");
    //            RenderTexture.active = rt;
    //            var readPixelsTimer = Stopwatch.StartNew();
    //            groundShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
    //            var readPixelsElapsed = readPixelsTimer.ElapsedMilliseconds;
    //            _camera.targetTexture = null;
    //            RenderTexture.active = null; // JC: added to avoid errors
    //            Destroy(rt);
    //            Destroy(_cameraObject);
    //            Color[] texColors = groundShot.GetPixels();
    //            int total = texColors.Length;
    //            float r = 0;
    //            float g = 0;
    //            float b = 0;


    //            for (int i = 0; i < total; i++)
    //            {
    //                r += texColors[i].r;
    //                g += texColors[i].g;
    //                b += texColors[i].b;
    //            }


    //            Color averageColor = new Color(r / total, g / total, b / total);
    //            print(averageColor);


    //            takeHiResShot = false;
    //            Debug.LogWarning("Elapsed milliseconds: " + stopwatch.ElapsedMilliseconds);
    //            Debug.LogWarning("Took " + readPixelsElapsed + " milliseconds to read pixels");
    //            Debug.LogWarning("Took " + (Time.realtimeSinceStartup - start).ToString("F2") +
    //                             " seconds to create snapshot");
    //        }
    //    }
    //}

    //[Serializable]
    //public class SomeDataIwantTosave : IConfigNode
    //{
    //    [KSPField(isPersistant = true), Persistent] private float MyFloat = 123.45f;
    //    [Persistent] public string PersistentString = "TestString";

    //    public void Load(ConfigNode node)
    //    {
            
    //    }

    //    public void Save(ConfigNode node)
    //    {
    //        Debug.LogError("SomeDataIwantToSave.Save");
    //        node.AddValue("Key", "Value");
    //    }

    //    public override string ToString()
    //    {
    //        return "ToStringSomeData";
    //    }
    //}

    //public class TestPersistentModule : PartModule
    //{
    //    [KSPField(isPersistant = true), Persistent] public SomeDataIwantTosave MyData = new SomeDataIwantTosave();
    //    [KSPField(isPersistant = true)] public float NormalSave = 444.44f;

    //    private void Start()
    //    {
    //        //var cfg = new ConfigNode();
    //        //Fields.Save(cfg);
    //        //print("Cfg: " + cfg.ToString());
    //        print("Cfg: " + ConfigNode.CreateConfigFromObject(this));
    //    }
    //}

    //public class CustomModuleInfo : PartModule, IModuleInfo
    //{
    //    public string GetModuleTitle()
    //    {
    //        return "My Custom Module";
    //    }

    //    public Callback<Rect> GetDrawModulePanelCallback()
    //    {
    //        return null; // could use this for graphics
    //    }

    //    public string GetPrimaryField()
    //    {
    //        return "Custom Module Info Here\n<b>Secret: Don't tell anyone</b>";
    //    }
    //}

//[KSPAddon(KSPAddon.Startup.Flight, false)]
public class PqsFence : MonoBehaviour
{
    private GameObject _postPrefab;
    private GameObject _fenceSegment;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1f);

        var cylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder); // 2 units high, 1 in diameter
        cylinder.transform.localScale = new Vector3(0.1f, 1f, 0.1f); // make it skinny: 2 units high, 0.1 in diameter

        _postPrefab = new GameObject("FencePost");
        cylinder.transform.parent = _postPrefab.transform;
        cylinder.transform.localPosition = new Vector3(0f, 2f * cylinder.transform.localScale.y*0.5f, 0f); // make the bottom of the fence post the point we're manipulating
        _postPrefab.SetLayerRecursive(15);
        _postPrefab.SetActive(false);


        var rectangleSegment = GameObject.CreatePrimitive(PrimitiveType.Cube); // 1x1x1

        // make it skinny and extend four meters in +x direction
        rectangleSegment.transform.localScale = new Vector3(4f, 0.1f, 0.1f);

        var segments = new[] {rectangleSegment, (GameObject) Instantiate(rectangleSegment)};

        _fenceSegment = new GameObject("FenceSegment");
           
        // the top of the fence post is at y=2, so let's place the segments at y=1.8 and y = 0.8
        for (int i = 0; i < 2; ++i)
        {
            segments[i].transform.parent = _fenceSegment.transform;
            segments[i].transform.localPosition = new Vector3(0f, 1.8f - i, 0f);
        }

        _fenceSegment.SetLayerRecursive(15);
        _fenceSegment.SetActive(false);


        var post = CreatePqsObject(_postPrefab, Vector3.up, Color.red); // place a fence post at the north pole
        CreatePqsObject(_fenceSegment, Vector3.up, Color.blue);
            
        for (int i = 0; i < 30; ++i)
        {
            // easy way: assuming one of your snap points is the position of an object already created
            // and positioned:
            //var radial = (post.transform.localPosition + new Vector3(4f, 0f, 0f)).normalized;

            // slightly less easy: convert radial direction into a world position, then add offset and normalize
            // to come up with radial vector
            var radial =
                ((Vector3d) post.repositionRadial*(post.sphere.radius + post.repositionRadiusOffset) +
                    new Vector3(4f, 0f, 0f)).normalized;

            post = CreatePqsObject(_postPrefab, radial, Color.black);
            CreatePqsObject(_fenceSegment, radial, Color.green);
        }
    }


    private PQSCity CreatePqsObject(GameObject prefab, Vector3 radial, Color color)
    {
        var item = (GameObject)Instantiate(prefab);
        var pqsCity = item.AddComponent<PQSCity>();

            
        pqsCity.repositionRadial = radial;
        pqsCity.repositionToSphereSurface = true;
        pqsCity.repositionToSphereSurfaceAddHeight = true;

        pqsCity.lod = new[] { new PQSCity.LODRange
        {
            renderers = item.GetComponentsInChildren<Renderer>().Select(r => r.gameObject).ToArray(),
            objects = new []{ item },
            visibleRange = 25000f
        }};
        pqsCity.lod[0].Setup();

        pqsCity.frameDelta = 1; //Unknown
        pqsCity.reorientToSphere = true; //adjust rotations to match the direction of gravity
        item.gameObject.transform.parent = FlightGlobals.GetHomeBody().transform;
        pqsCity.sphere = FlightGlobals.GetHomeBody().pqsController;
        pqsCity.order = 100;
        pqsCity.modEnabled = true;
        pqsCity.OnSetup();
        pqsCity.Orientate();
        item.SetActive(true);

        item.GetComponentsInChildren<Renderer>().ToList().ForEach(r => r.material.color = color);

        print("Created new object at " + pqsCity.transform.localPosition);

        return pqsCity;
    }
}


    //public class AddAttachNodePartModule : PartModule
    //{
    //    private readonly ILog _log = new DebugLog("AddAttachNodePartModule");

    //    public override void OnAwake()
    //    {
    //        _log.Warning("AddAttachNodePartModule awake");
    //        base.OnAwake();

    //        var model = transform.Find("model");

    //        part.attachNodes.ForEach(an => _log.Normal("AttachNode: " + an.id + ", " + an.attachMethod));

    //        //part.attachNodes.ForEach(an =>
    //        //{
    //        //    _log.Normal("id: " + an.id);

    //        //    //if (an.parent == part.transform)
    //        //    //    _log.Warning("wtf it's on part");
    //        //    //else if (an.nodeTransform.parent == model)
    //        //    //    _log.Warning("yep attach nodes are as expected");
    //        //    //else _log.Error("oh shit I ahve no idea what's going on");
    //        //});

    //        var location = new GameObject("customAttach");
    //        location.transform.parent = model;
    //        location.transform.localPosition = Vector3.up*3f;

    //        //if (part.attachNodes.Any(an => an.id == "top"))
    //        //    part.attachNodes.Remove(part.attachNodes.First(an => an.id == "top"));

    //        //part.attachRules.allowStack = true;
    //        //part.attachRules.stack = true;

    //        part.attachNodes.Add(new AttachNode("custom", location.transform, 1, AttachNodeMethod.HINGE_JOINT) { owner = part, orientation = Vector3.up, position = location.transform.position, originalPosition = location.transform.position, originalOrientation = Vector3.up});
    //        //var cfg = new ConfigNode();
    //        //cfg.AddValue("transform", "customAttach");

    //        //part.AddAttachNode(cfg);

    //        part.gameObject.PrintComponents(_log);
    //    }

    //    private void Start()
    //    {
    //        var model = transform.Find("model");
    //        part.attachNodes.ForEach(an =>
    //        {
    //            _log.Normal("id: " + an.id);
    //            _log.Normal("position: " + an.position);
    //            _log.Normal("original position: " + an.originalPosition);

    //            //if (an.nodeTransform == null) return;

    //            //if (an.nodeTransform.parent == part.transform)
    //            //    _log.Warning("wtf it's on part");
    //            //else if (an.nodeTransform.parent == model)
    //            //    _log.Warning("yep attach nodes are as expected");
    //            //else _log.Error("oh shit I ahve no idea what's going on");
    //        });
    //    }

    //    private void Update()
    //    {
    //        if (Input.GetKeyDown(KeyCode.P))
    //        {
    //            part.gameObject.PrintComponents(_log);
    //        }
    //    }
    //}


    //[KSPAddon(KSPAddon.Startup.EditorAny, false)]
    //public class IconPrefabInstaller : MonoBehaviour
    //{
    //    private class Echoer : MonoBehaviour
    //    {
    //        private void Update()
    //        {
    //            if (Input.GetKeyDown(KeyCode.O))
    //                print("Echo!");
    //        }
    //    }
    //    private void Start()
    //    {
    //        print("Installing echoer");
    //        EditorLogic.fetch.attachNodePrefab.AddComponent<Echoer>();
    //    }
    //}

    //[KSPAddon(KSPAddon.Startup.SpaceCentre, true)]
    //public class InstallPartModule : MonoBehaviour
    //{
    //    private void Start()
    //    {
    //        DontDestroyOnLoad(this);

    //        var ap = PartLoader.getPartInfoByName("mk1pod");

    //        var pm = ap.partPrefab.gameObject.AddComponent<AddAttachNodePartModule>();

    //        //typeof (PartModule).GetMethod("Awake", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(pm, null);
    //    }

    //    private void OnDestroy()
    //    {
    //        // uninstall
    //        try
    //        {
    //            var ap = PartLoader.getPartInfoByName("mk1pod");
    //            Destroy(ap.partPrefab.gameObject.GetComponent<AddAttachNodePartModule>());

    //            var model = ap.partPrefab.transform.Find("model");

    //            for (int i = 0; i < model.childCount;)
    //            {
    //                if (model.GetChild(i).name == "customAttach")
    //                {
    //                    model.GetChild(i).parent = null;
    //                    Destroy(model.GetChild(i));
    //                }
    //                else ++i;
    //            }

    //            ap.partPrefab.attachNodes.Remove(ap.partPrefab.attachNodes.First(an => an.id == "custom"));

    //        }
    //        catch (Exception e)
    //        {
    //            Debug.LogError("Failed to remove PartModule");
    //        }
    //    } 
    //}
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
//        public void Deserialize(ConfigNode node)
//        {
//            _log.Warning("Deserialize, string is " + MyString);
//        }

//        public void Serialize(ConfigNode node)
//        {
//            _log.Warning("Serialize");
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