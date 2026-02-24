using System.IO;
using System.Linq;
using UnityEngine;

public class MarbleInfo : MonoBehaviour
{
    public static MarbleInfo instance;
    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [Header("Material")]
    public Material[] customMarbleRegularMaterial;
    public Material[] customMarbleTeleportMaterial;
    public Material[] defaultMarbleTeleportMaterial;
    [Header("Mesh")]
    public GameObject[] defaultMarbleMesh;
    public GameObject[] customMarbleMesh;
    [Header("Radius")]
    public float[] defaultMarbleRadius;
    public float[] customMarbleRadius;

    void Start()
    {
        InitMaterial();
    }

    void InitMaterial()
    {
        for (int i = 0; i < customMarbleRegularMaterial.Length - 5 ; i++)
        {
            // Try jpg first, then png
            string jpgPath = Path.Combine(
                Application.streamingAssetsPath,
                "marble/custom_marbles/CustomMarble_" + (i + 1).ToString("00") + ".jpg"
            );

            string pngPath = Path.Combine(
                Application.streamingAssetsPath,
                "marble/custom_marbles/CustomMarble_" + (i + 1).ToString("00") + ".png"
            );

            string imagePath = null;

            if (File.Exists(jpgPath))
                imagePath = jpgPath;
            else if (File.Exists(pngPath))
                imagePath = pngPath;

            Texture2D texture = null;

            if (!string.IsNullOrEmpty(imagePath))
            {
                byte[] imageData = File.ReadAllBytes(imagePath);

                texture = new Texture2D(2, 2, TextureFormat.RGBA32, false);
                texture.LoadImage(imageData);

                Color32[] pixels = texture.GetPixels32();
                for (int j = 0; j < pixels.Length; j++)
                    pixels[j].a = 255;

                texture.SetPixels32(pixels);
                texture.Apply();
            }

            if (texture)
            {
                customMarbleRegularMaterial[i].mainTexture = texture;
                customMarbleTeleportMaterial[i].mainTexture = texture;
            }
        }
    }

    public void ApplyMesh()
    {
        int selectedMarbleIndex = PlayerPrefs.GetInt("SelectedMarbleIndex", 0);
        MarbleType marbleType = PlayerPrefs.GetInt("DefaultMarbleIsSelected", 0) == 0 ? MarbleType.Default : MarbleType.Custom;
        Transform marble = Marble.instance.transform;

        if (marbleType == MarbleType.Default)
        {
            int length = defaultMarbleMesh[selectedMarbleIndex].GetComponent<MeshRenderer>().sharedMaterials.Length;

            Material[] mats = new Material[length];
            for (int i = 0; i < length; i++)
                mats[i] = defaultMarbleTeleportMaterial.FirstOrDefault(o => o.name == defaultMarbleMesh[selectedMarbleIndex].GetComponent<MeshRenderer>().sharedMaterial.name);

            Marble.instance.teleportMesh.GetComponent<MeshRenderer>().sharedMaterials = mats;

            Marble.instance.teleportMesh.GetComponent<MeshFilter>().sharedMesh = defaultMarbleMesh[selectedMarbleIndex].GetComponent<MeshFilter>().sharedMesh;

            Marble.instance.normalMesh.GetComponent<MeshRenderer>().sharedMaterials = defaultMarbleMesh[selectedMarbleIndex].GetComponent<MeshRenderer>().sharedMaterials;
            Marble.instance.normalMesh.GetComponent<MeshFilter>().sharedMesh = defaultMarbleMesh[selectedMarbleIndex].GetComponent<MeshFilter>().sharedMesh;

            float radius = defaultMarbleRadius[selectedMarbleIndex] * Mathf.Max(
                marble.lossyScale.x,
                marble.lossyScale.y,
                marble.lossyScale.z
            );

            Marble.instance.GetComponent<SphereCollider>().radius = defaultMarbleRadius[selectedMarbleIndex];
            Marble.instance.GetComponent<Movement>().marbleRadius = radius;
        }
        else
        {
            Marble.instance.teleportMesh.GetComponent<MeshRenderer>().sharedMaterial = customMarbleTeleportMaterial.FirstOrDefault(o => o.name == customMarbleMesh[selectedMarbleIndex].GetComponent<MeshRenderer>().sharedMaterial.name);
            Marble.instance.teleportMesh.GetComponent<MeshFilter>().sharedMesh = customMarbleMesh[selectedMarbleIndex].GetComponent<MeshFilter>().sharedMesh;

            Marble.instance.normalMesh.GetComponent<MeshRenderer>().sharedMaterials = customMarbleMesh[selectedMarbleIndex].GetComponent<MeshRenderer>().sharedMaterials;
            Marble.instance.normalMesh.GetComponent<MeshFilter>().sharedMesh = customMarbleMesh[selectedMarbleIndex].GetComponent<MeshFilter>().sharedMesh;

            float radius = customMarbleRadius[selectedMarbleIndex] * Mathf.Max(
                marble.lossyScale.x,
                marble.lossyScale.y,
                marble.lossyScale.z
            );

            Marble.instance.GetComponent<SphereCollider>().radius = customMarbleRadius[selectedMarbleIndex];
            Marble.instance.GetComponent<Movement>().marbleRadius = radius;
        }
    }
}
