//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection;
//using System.Text;
//using Contracts.Agents.Mentalities;
//using ReeperCommon.Extensions;
//using ReeperCommon.Logging.Implementations;
//using UnityEngine;
//using Object = UnityEngine.Object;

//namespace TestProject
//{
//    public class DiffuseMapColorProvider
//    {
//        private readonly CelestialBody _body;

//        public DiffuseMapColorProvider(CelestialBody body)
//        {
//            if (body == null) throw new ArgumentNullException("body");
//            _body = body;

//            body.gameObject.PrintComponents(new DebugLog(body.bodyName));
//        }

//        public Color Get(double lat, double lon)
//        {
//            return Color.clear;
//        }
//    }

//    [Serializable, PersistentLinkable]
//    public class DustEffectInfo : IConfigNode
//    {
//        //[Persistent]
//        public AudioClip Noise;

//        //[Persistent]
//        public float Volume;

//        public void Load(ConfigNode node)
//        {
            
//        }

//        public void Save(ConfigNode node)
//        {
//            new DebugLog().Normal("DustEffectInfo.Save - {0}", node.ToString());
//            node.AddValue("testvalue", "result");
//        }
//    }

//    public class BiomeDustColorInfo
//    {
//        [Persistent]
//        public string Name;

//        [Persistent]
//        public Color Color = Color.clear;

//        //Serializable
//        //SerializeField]
//        [KSPField(isPersistant = true)]
//        public DustEffectInfo Effect = new DustEffectInfo();
//    }



//    public class BodyDustColorInfo : IPersistenceLoad
//    {
//        [Persistent]
//        public string Name;

//        [Persistent]
//        public Color DefaultColor = Color.clear;

//        [Persistent, KSPField]
//        public List<BiomeDustColorInfo> BiomeColors = new List<BiomeDustColorInfo>();

//        private Dictionary<string, Color> _colorDictionary = new Dictionary<string, Color>();

//        public void PersistenceLoad()
//        {
//            _colorDictionary = BiomeColors.ToDictionary(ci => ci.Name, ci => ci.Color);
//        }

//        public Color GetDustColor(string name)
//        {
//            Color color;

//            return _colorDictionary.TryGetValue(name, out color) ? color : DefaultColor;
//        }
//    }

//    [Serializable]
//    public class DustColorRepository : IPersistenceLoad, IPersistenceSave, IConfigNode
//    {
//        [Persistent]
//        private List<BodyDustColorInfo> BodyDustColors = new List<BodyDustColorInfo>();
//        [Persistent]
//        private Color DefaultColor = Color.clear;

//        private Dictionary<CelestialBody,
//            BodyDustColorInfo>
//            _dustColors = new Dictionary<CelestialBody, BodyDustColorInfo>();


//        public Color GetDustColor(CelestialBody body, double lat, double lon)
//        {
//            BodyDustColorInfo biomeColors;

//            if (!_dustColors.TryGetValue(body, out biomeColors))
//                return DefaultColor;

//            var biome = FlightGlobals.ActiveVessel == null
//                ? string.Empty
//                : (string.IsNullOrEmpty(FlightGlobals.ActiveVessel.landedAt)
//                    ? ScienceUtil.GetExperimentBiome(body, lat, lon)
//                    : Vessel.GetLandedAtString(FlightGlobals.ActiveVessel.landedAt));

//            return biomeColors.GetDustColor(biome);
//        }


//        public void PersistenceLoad()
//        {
//            _dustColors = BodyDustColors
//                .Where(info => FlightGlobals.Bodies.Any(b => b.bodyName == info.Name))
//                .ToDictionary(
//                info => FlightGlobals.Bodies.Single(b => b.bodyName == info.Name), c => c);
//        }


//        public void PersistenceSave()
//        {
//            if (!BodyDustColors.Any())
//            {

//                // generate default config
//                BodyDustColors = FlightGlobals.Bodies.Where(cb => cb.BiomeMap != null).Select(b => new BodyDustColorInfo
//                {
//                    DefaultColor = this.DefaultColor,
//                    Name = b.bodyName,
//                    BiomeColors = b.BiomeMap.Attributes
//                        .GroupBy(attr => attr.name)
//                        .Select(group => group.First())
//                        .Select(attr => new BiomeDustColorInfo { Color = attr.mapColor, Name = attr.name, Effect = new DustEffectInfo()}).ToList()
//                }).ToList();
//            }
//        }

//        public void Load(ConfigNode node)
//        {
//            throw new NotImplementedException();
//        }

//        public void Save(ConfigNode node)
//        {
//            throw new NotImplementedException();
//        }
//    }


//    [KSPAddon(KSPAddon.Startup.Flight, false)]
//    public class SomeAddonOrScenarioModuleOrWhatever : MonoBehaviour
//    {
//        private DustColorRepository _dustRepository = new DustColorRepository();

//        private void Awake()
//        {
//            // save default config
//            // ConfigNode.Save sometimes strips out the root node which could be a problem if it's empty,
//            // leading to a blank cfg = crashes KSP loader
//            var defaultConfig = ConfigNode.CreateConfigFromObject(_dustRepository);
//            defaultConfig.name = "DustColorDefinitions";

//            //System.IO.File.WriteAllText(KSPUtil.ApplicationRootPath + "GameData/DustColorDefinitions.cfg", defaultConfig.ToString());
//            System.IO.File.WriteAllText(KSPUtil.ApplicationRootPath + "GameData/testAdvanced.cfg", defaultConfig.ToString());

//            var definition = GameDatabase.Instance.GetConfigNodes("DustColorDefinitions").Single();

//            if (!ConfigNode.LoadObjectFromConfig(_dustRepository, definition))
//                print("ERROR: failed to load dust definitions");
//        }


//        private void Update()
//        {
//            //print("Current dust color: " +
//            //        _dustRepository.GetDustColor(FlightGlobals.ActiveVessel.mainBody,
//            //            FlightGlobals.ActiveVessel.latitude, FlightGlobals.ActiveVessel.longitude));

//            //print("LandedAt: " + Vessel.GetLandedAtString(FlightGlobals.ActiveVessel.landedAt));
//        }
//    }


//    //[KSPAddon(KSPAddon.Startup.EditorAny, false)]
//    public class CheckRootModules : MonoBehaviour
//    {


//        private void Awake()
//        {
//            //if (EditorLogic.RootPart != null)
//            //EditorLogic.RootPart.FindModulesImplementing<PartModule>()
//            //    .ToList()
//            //    .ForEach(m => print("Module: " + m.GetType().FullName));


//        }

//        private void Update()
//        {
//            if (Input.GetKeyDown(KeyCode.Keypad8))
//            {
//                print("Dumping loaded parts now ...");

//                var loadedParts = Object.FindObjectsOfType(typeof(Part)).Cast<Part>().ToList();

//                print(loadedParts.Count + " FindObjectsOfType returned");

//                // Bit tricky here: loadedParts is looking for loose parts so if the parts are actually attached to each
//                // other via parenting instead of joints (such as when building a ship in the editor), we'll only find
//                // the top-level ones! Better look for children too
//                loadedParts
//                    .SelectMany(p => p.gameObject.GetComponentsInChildren<Part>(true))
//                    .Where(p => !ReferenceEquals(p.gameObject, p.partInfo.partPrefab.gameObject))
//                    .ToList()
//                    .ForEach(p => print("Loaded part: " + p.name + ", " + p.flightID));
//            }
//        }
//    }


//    //[KSPAddon(KSPAddon.Startup.Flight, false)]
//    public class DustColorBulb : MonoBehaviour
//    {
//        private const float HoverHeight = 10f;

//        private GameObject _bulb;
//        private Material _bulbMaterial;

//        private System.Collections.IEnumerator Start()
//        {
//            while (!FlightGlobals.ready || FlightGlobals.ActiveVessel == null || !FlightGlobals.ActiveVessel.loaded)
//                yield return 0;

//            _bulb = GameObject.CreatePrimitive(PrimitiveType.Sphere);

//            _bulb.transform.parent = FlightGlobals.ActiveVessel.ReferenceTransform;
//            _bulb.transform.localPosition = FlightGlobals.ActiveVessel.GetWorldPos3D() + Vector3.up * HoverHeight;

//            PartLoader.StripComponent<Collider>(_bulb);

//            var tex = new Texture2D(1, 1);
//            tex.SetPixel(0, 0, Color.white);
//            tex.Apply();


//            _bulbMaterial = _bulb.renderer.material;
            
//            _bulbMaterial.mainTexture = tex;
//            _bulbMaterial.color = Color.red;
//            _bulbMaterial.shader = Shader.Find("KSP/Unlit");
//        }


//        private void Update()
//        {
//            _bulb.transform.position = _bulb.transform.parent.position + FlightGlobals.upAxis*HoverHeight;

//            var ray = new Ray(_bulb.transform.position, -FlightGlobals.upAxis);
//            RaycastHit hitInfo;

//            if (Physics.Raycast(ray, out hitInfo, HoverHeight + 1f, 1 << 15))
//            {
//                if (_bulbMaterial.color == Color.green) return;

//                _bulbMaterial.color = Color.green;

//                var log = new DebugLog("HitInfo");
                    
//                hitInfo.transform.gameObject.PrintComponents(log);

//                var r = hitInfo.transform.renderer;

//                if (r == null)
//                    log.Warning("r is null");

//                if (r.material == null) log.Warning("r.mat is null");

//                if (r.material.mainTexture == null) log.Warning("r.mat.tex is null");

//                var pq = hitInfo.transform.GetComponent<PQ>();

//                if (pq == null) log.Warning("pq is null");

//                if (pq.meshRenderer == null) log.Warning("pq.meshRenderer is null");

               
//                if (pq.meshRenderer.material == null) log.Warning("pq.r.mat is null");
//                log.Normal("pq.meshR shader: " + pq.meshRenderer.material.shader.name);
//                log.Normal("pq.r shader: " + hitInfo.transform.renderer.material.shader.name);

     
//                if (pq.meshRenderer.material.mainTexture == null) log.Warning("pq.r.mat.tex is null");

//                //Transform top = hitInfo.transform;

//                //while (top.parent != null)
//                //    top = top.parent;

//                //top.gameObject.PrintComponents(log);

//                var pqs = hitInfo.transform.GetComponentInParent<PQS>();

//                log.Normal("children:");

//                foreach (Transform t in hitInfo.transform)
//                    t.gameObject.PrintComponents(log);

//                log.Normal("end children");
//                //hitInfo.transform.GetComponent<PQ>().meshRenderer.sharedMaterial.shaderKeywords.ToList()
//                //    .ForEach(kw => new DebugLog("Keyword").Normal(kw));

//                ////hitInfo.transform.renderer.material.mainTexture.As2D()
//                ////    .CreateReadable()
//                ////    .SaveToDisk("pq_renderer_tex.png");
//            }
//            else
//            {
//                _bulbMaterial.color = Color.red;
//            }
//        }


//        private void OnPluginReloadRequested()
//        {
//            print("DustColorBulb: reload requested");
//            Destroy(_bulb);
//            Destroy(_bulbMaterial);
//        }
//    }
//}
