using Antlr4.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

namespace TS
{
    public class MissionImporterPreview : MonoBehaviour
    {
        public List<TSObject> MissionObjects;
        public string thisMission;

        [Header("Prefabs")]
        public GameObject interiorPrefab;
        public GameObject movingPlatformPrefab;
        public GameObject triggerGoToTarget;
        public GameObject inBoundsTrigger;
        public GameObject outOfBoundsTrigger;
        public GameObject helpTriggerInstance;
        public GameObject teleportTrigger;
        public GameObject destinationTrigger;
        [Space]
        public GameObject finishSignPrefab;
        public GameObject signPlainPrefab;
        public GameObject signPlainUpPrefab;
        public GameObject signPlainDownPrefab;
        public GameObject signPlainLeftPrefab;
        public GameObject signPlainRightPrefab;
        public GameObject signCautionPrefab;
        public GameObject signCautionCautionPrefab;
        public GameObject signCautionDangerPrefab;
        public GameObject signPrefab;
        public GameObject signDownPrefab;
        public GameObject signDownSidePrefab;
        public GameObject signSidePrefab;
        public GameObject signUpPrefab;
        public GameObject signUpSidePrefab;
        [Space]
        public GameObject gemPrefab;
        public GameObject antiGravityPrefab;
        public GameObject superJumpPrefab;
        public GameObject superSpeedPrefab;
        public GameObject superBouncePrefab;
        public GameObject shockAbsorberPrefab;
        public GameObject gyrocopterPrefab;
        public GameObject timeTravelPrefab;
        public GameObject randomPowerupPrefab;
        public GameObject easterEggPrefab;
        [Space]
        public GameObject trapdoorPrefab;
        public GameObject roundBumperPrefab;
        public GameObject triangleBumperPrefab;
        public GameObject ductFanPrefab;
        public GameObject tornadoPrefab;
        public GameObject oilSlickPrefab;
        public GameObject landMinePrefab;
        public GameObject nukePrefab;
        public GameObject magnetPrefab;
        [Space]
        public GameObject teleportPad;
        public GameObject checkpointPrefab;
        [Space]
        public GameObject[] staticShapes;

        [Header("References")]
        public GameObject globalMarble;
        public GameObject startPad;
        public GameObject finishPad;
        //public Light directionalLight;

        List<GameObject> checkpoints = new List<GameObject>();
        List<GameObject> destinationTriggers = new List<GameObject>();
        List<GameObject> teleportTriggers = new List<GameObject>();

        private float[] sunColor;

        void Start()
        {
            //MarbleInfo.instance.ApplyMesh();
            //directionalLight = FindObjectsOfType<directionalLight>();
            //ImportMission();
        }

        public void setInteriorPrefab(GameObject interior) {
            interiorPrefab = interior;
        }

        public void setThisMission(string mis){
            thisMission = mis;
            ImportMission();
        }

        void ImportMission()
        {
            if (string.IsNullOrEmpty(thisMission))
                return;

            var lexer = new TSLexer(
                new AntlrFileStream(Path.Combine(Application.streamingAssetsPath, thisMission))
            );
            var parser = new TSParser(new CommonTokenStream(lexer));
            var file = parser.start();

            if (parser.NumberOfSyntaxErrors > 0)
            {
                Debug.LogError("Could not parse mission file");
                return;
            }

            MissionObjects = new List<TSObject>();

            foreach (var decl in file.decl())
            {
                var objectDecl = decl.stmt()?.expression_stmt()?.stmt_expr()?.object_decl();
                if (objectDecl == null)
                    continue;

                MissionObjects.Add(ProcessObject(objectDecl));
            }

            if (MissionObjects.Count == 0)
                return;

            var mission = MissionObjects[0];
            var children = mission.GetFirstChildrens();

            foreach (var obj in mission.RecursiveChildren())
            {
                if (obj.ClassName == "Sun")
                {
                    var direction = ConvertDirection(ParseVectorString(obj.GetField("direction")));
                    sunColor = ParseVectorString(obj.GetField("color"));
                    var ambient = ParseVectorString(obj.GetField("ambient"));

                    //directionalLight.transform.localRotation = direction;
                    //directionalLight.color = new Color(sunColor[0], sunColor[1], sunColor[2], 1f);

                    RenderSettings.ambientLight = new Color(ambient[0], ambient[1], ambient[2], 1f);
                }

                //Gem
                else if (obj.ClassName == "Item")
                {
                    string objectName = obj.GetField("dataBlock");

                    
                }

                //Interior
                else if (obj.ClassName == "InteriorInstance")
                {
                    var gobj = Instantiate(interiorPrefab, transform, false);
                    gobj.name = "InteriorInstance";

                    var position = ConvertPoint(ParseVectorString(obj.GetField("position")));
                    var rotation = ConvertRotation(ParseVectorString(obj.GetField("rotation")));
                    var scale = ConvertScale(ParseVectorString(obj.GetField("scale")));

                    gobj.transform.localPosition = position;
                    gobj.transform.localRotation = rotation;
                    gobj.transform.localScale = scale;

                    var difPath = ResolvePath(obj.GetField("interiorFile"), thisMission);
                    var dif = gobj.GetComponent<Dif>();
                    dif.filePath = difPath;
                    if (!dif.GenerateMesh(-1)) {
                        Destroy(gobj.gameObject);
                    } else {
                        //things ok?
                    }
                        

                    //if scale has component with 0 value, remove all colliders
                    if(scale.x == 0 || scale.y == 0 || scale.z == 0)
                    {
                        MeshCollider[] meshColliders = gobj.GetComponentsInChildren<MeshCollider>(true);
                        foreach (var mc in meshColliders)
                            Destroy(mc);
                    }
                }
                
                //spawn spheres
                else if (obj.ClassName == "SpawnSphere")
                {
                    string objectName = obj.GetField("dataBlock");
                    if(objectName == "CameraSpawnSphereMarker"){
                        GameObject gobj = Instantiate(new GameObject(), transform, false);
                        gobj.name = "CameraSpawnSphereMarker";

                        var position = ConvertPoint(ParseVectorString(obj.GetField("position")));
                        var rotation = ConvertRotation(ParseVectorString(obj.GetField("rotation")));
                        var scale = ConvertScale(ParseVectorString(obj.GetField("scale")));

                        var localScale = gobj.transform.localScale;

                        gobj.transform.localPosition = position;
                        gobj.transform.localRotation = rotation * Quaternion.Euler(90f, 0f, 0f);
                        gobj.transform.localScale = new Vector3(scale.x * localScale.x, scale.y * localScale.y, scale.z * localScale.z);
                    }
                }

                //Shapes
                else if (obj.ClassName == "StaticShape")
                {
                    string objectName = obj.GetField("dataBlock");
                }

                else if (obj.ClassName == "TSStatic")
                {
                    string objectName = obj.GetField("shapeName");
                    objectName = Path.GetFileNameWithoutExtension(objectName);

                    
                }

                //Triggers
                else if (obj.ClassName == "Trigger")
                {
                    string objectName = obj.GetField("dataBlock");

                    

                    

                    

                    
                }

            }

            //assigning destination triggers to teleport
            foreach (GameObject go in teleportTriggers)
            {
                Teleport tele = go.GetComponent<Teleport>();
                string destination = tele.destinationGameObjectName;
                foreach (GameObject dest in destinationTriggers)
                {
                    if (dest.name == destination)
                    {
                        tele.destination = dest;
                        break;
                    }
                }
                tele.InitTeleporter();
            }

            //assigning checkpoint triggers
            foreach (var obj in mission.RecursiveChildren())
            {
                if (obj.ClassName == "Trigger" && obj.GetField("dataBlock") == "CheckpointTrigger")
                {
                    foreach (GameObject go in checkpoints)
                    {
                        if (obj.GetField("respawnPoint") == go.name)
                        {
                            var cpTrigger = go.transform.Find("Trigger");

                            var position = ConvertPoint(ParseVectorString(obj.GetField("position")));
                            var rotation = ConvertRotation(ParseVectorString(obj.GetField("rotation")), false);
                            var scale = ConvertScale(ParseVectorString(obj.GetField("scale")));

                            var polyhedronScale = PolyhedronToBoxSize(ParseVectorString(obj.GetField("polyhedron")));

                            cpTrigger.transform.position = position;
                            cpTrigger.transform.rotation = Quaternion.Euler(-90f, 0f, 0f);
                            cpTrigger.transform.localScale = Vector3.Scale(scale, polyhedronScale);

                            cpTrigger.GetComponent<BoxCollider>().enabled = true;
                            break;
                        }
                    }
                }
            }

            //StartCoroutine(DelayBeforeRespawn());
        }

        IEnumerator DelayBeforeRespawn()
        {
            while (!GameManager.instance.startPad)
                yield return null;

            GameUIManager.instance.Init();

            globalMarble.GetComponent<Movement>().GenerateMeshData();

            Time.timeScale = 1f;
            GameManager.instance.InitGemCount();

            GameManager.instance.SetSoundVolumes();
            GameManager.instance.PlayLevelMusic();

            //directionalLight.GetComponent<Light>().shadows = PlayerPrefs.GetInt("Graphics_Shadow", 1) == 1 ? LightShadows.Soft : LightShadows.None;
            //directionalLight.intensity *= ConvertIntensity(sunColor);

            AsyncOperation unloadOp = SceneManager.UnloadSceneAsync("Loading");
            while (!unloadOp.isDone)
                yield return null;

            SceneManager.SetActiveScene(SceneManager.GetSceneByName(MissionInfo.instance.skybox));

            CameraController.instance.GetComponent<Camera>().enabled = true;
            GameUIManager.instance.GetComponent<Canvas>().enabled = true;

            //disable cursor visibility
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            Invoke(nameof(EnableSounds), 0.1f);

            Marble.onRespawn?.Invoke();
        }

        void EnableSounds()
        {
            var sounds = Marble.instance.transform.Find("Sounds");
            var audioSources = sounds.GetComponentsInChildren<AudioSource>(true);
            var rolling = audioSources.First(a => a.name == "Rolling");
            var sliding = audioSources.First(a => a.name == "Sliding");
            rolling.Play();
            sliding.Play();
        }

        // -------------------------    
        // Conversion helpers
        // -------------------------
        float ConvertIntensity(float[] torqueColor)
        {
            // Torque stores brightness in RGB
            float intensity = Mathf.Max(
                torqueColor[0],
                torqueColor[1],
                torqueColor[2]
            );

            if (intensity <= 0f)
                intensity = 1f;

            return intensity;
        }

        Quaternion ConvertDirection(float[] torqueDir)
        {
            // Torque Z-up → Unity Y-up
            Vector3 unityDir = new Vector3(
                torqueDir[0],
                torqueDir[2],
                torqueDir[1]
            );

            unityDir.Normalize();

            // Unity directional lights shine along -forward
            return Quaternion.LookRotation(unityDir, Vector3.up);
        }

        private Vector3 ConvertPoint(float[] p)
        {
            return new Vector3(p[0], p[2], p[1]);
        }

        private Quaternion ConvertRotation(float[] torqueRotation, bool additionalRotate = true)
        {
            // Torque point is an angle axis in torquespace
            float angle = torqueRotation[3];
            Vector3 axis = new Vector3(torqueRotation[0], -torqueRotation[1], torqueRotation[2]);
            Quaternion rot = Quaternion.AngleAxis(angle, axis);

            if (additionalRotate)
                rot = Quaternion.Euler(-90.0f, 0, 0) * rot;

            return rot;
        }

        private Vector3 ConvertScale(float[] s)
        {
            return new Vector3(s[0], s[1], s[2]);
        }

        private Vector3 PolyhedronToBoxSize(float[] poly)
        {
            if (poly == null || poly.Length != 12)
                throw new ArgumentException("Polyhedron must be 12 floats: origin + 3 edge vectors");

            Vector3 d1 = new Vector3(poly[3], poly[4], poly[5]);
            Vector3 d2 = new Vector3(poly[6], poly[7], poly[8]);
            Vector3 d3 = new Vector3(poly[9], poly[10], poly[11]);

            return new Vector3(
                Mathf.Abs(d1.x) + Mathf.Abs(d2.x) + Mathf.Abs(d3.x),
                Mathf.Abs(d1.y) + Mathf.Abs(d2.y) + Mathf.Abs(d3.y),
                Mathf.Abs(d1.z) + Mathf.Abs(d2.z) + Mathf.Abs(d3.z)
            );
        }


        private static readonly CultureInfo Invariant = CultureInfo.InvariantCulture;

        private float[] ParseVectorString(string vs)
        {
            return vs
                .Split(' ')
                .Select(s => float.Parse(s, Invariant))
                .ToArray();
        }

        private string ResolvePath(string assetPath, string misPath)
        {
            if (string.IsNullOrEmpty(assetPath))
                return assetPath;

            // Normalize slashes first
            assetPath = assetPath.Replace('\\', '/');

            // Remove trailing quote if present
            if (assetPath.EndsWith("\""))
                assetPath = assetPath.Substring(0, assetPath.Length - 1);

            // Remove leading slashes
            assetPath = assetPath.TrimStart('/');

            // --- Resolve special prefixes ---
            if (assetPath[0] == '.')
            {
                // Relative to mission file
                assetPath = Path.GetDirectoryName(misPath).Replace('\\', '/')
                            + assetPath.Substring(1);
            }
            else
            {
                // Replace anything before first '/' with "marble"
                int slash = assetPath.IndexOf('/');
                assetPath = slash >= 0
                    ? "marble" + assetPath.Substring(slash)
                    : "marble/" + assetPath;
            }

            // --- Fix lbinteriors_* folders ---
            assetPath = assetPath
                .Replace("/lbinteriors_mbp/", "/interiors_mbp/")
                .Replace("/lbinteriors_mbg/", "/interiors_mbg/");

            return assetPath;
        }


        public static TSObject ProcessObject(TSParser.Object_declContext objectDecl)
        {
            var obj = ScriptableObject.CreateInstance<TSObject>();

            obj.ClassName = objectDecl.class_name_expr().GetText();
            obj.Name = objectDecl.object_name().GetText();

            var block = objectDecl.object_declare_block();
            if (block == null)
                return obj;

            foreach (var assignList in block.slot_assign_list())
            {
                foreach (var slot in assignList.slot_assign())
                {
                    var key = slot.children[0].GetText().ToLower();
                    var value = slot.expr().GetText();

                    var str = slot.expr().STRATOM();
                    if (str != null)
                        value = str.GetText().Substring(1, value.Length - 2);

                    // Torque allows duplicate keys; last one wins
                    if (obj.Fields.ContainsKey(key))
                        obj.Fields[key] = value;
                    else
                        obj.Fields.Add(key, value);
                }
            }

            foreach (var sub in block.object_decl_list())
            {
                foreach (var subDecl in sub.object_decl())
                {
                    var child = ProcessObject(subDecl);
                    child.Parent = obj;
                    obj.Children.Add(child);
                }
            }

            return obj;
        }

    }
}