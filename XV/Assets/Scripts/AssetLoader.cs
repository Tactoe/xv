using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class AssetLoader : MonoBehaviour {

    public static AssetLoader Instance;    

    [SerializeField]
    FillAssetScrollView m_AssetScrollView;

    [SerializeField]
    List<GameObject> m_Assets;
    
    [SerializeField]
    List<Sprite> m_Thumbnails;

    void Awake()
    {
        Instance = this;
    }

    void Start() {
        string bundlePath = Path.Combine(Application.streamingAssetsPath, "Bundles", "export");
        if (Directory.Exists(bundlePath))
        {
            AssetBundle bundle = AssetBundle.LoadFromFile(bundlePath);
            if (bundle == null) {
                Debug.Log("Failed to load AssetBundle!");
                return;
            }
            GameObject[] customModels = bundle.LoadAllAssets<GameObject>();
            Sprite[] customThumbnails = bundle.LoadAllAssets<Sprite>();
            foreach (GameObject model in customModels)
            {
                m_Assets.Add(model);
            }
            foreach (Sprite thumbnail in customThumbnails)
            {
                m_Thumbnails.Add(thumbnail);
            }
        }
        m_AssetScrollView.Init(m_Assets, m_Thumbnails);
    }
    
    #if UNITY_EDITOR
    [ContextMenu("GenerateThumbnails")]
    public void GenerateThumbnails()
    {
        var dirPath = Application.dataPath + "/Sprites/Thumbnails/";
        if(!Directory.Exists(dirPath))
        {
            Directory.CreateDirectory(dirPath);
        }
        foreach (GameObject asset in m_Assets)
        {
            Texture2D texture = AssetPreview.GetAssetPreview(asset);
            int timeout = 0;
            while (texture == null && timeout < 100)
            {
                texture = AssetPreview.GetAssetPreview(asset);
                System.Threading.Thread.Sleep(15);
                timeout++;
            }
            byte[] bytes = texture.EncodeToPNG();
            File.WriteAllBytes(dirPath + asset.name + ".png", bytes);
        }
    }
    #endif
}