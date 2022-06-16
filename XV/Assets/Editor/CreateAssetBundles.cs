using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class CreateAssetBundles
{
    static void Test()
    {
        List<GameObject> assets = new List<GameObject>();
        string[] folders = new string[] {"Assets/Prefabs/Export"};
        string[] ok = AssetDatabase.FindAssets("", folders);
        List<GameObject> makeThumbnail = new List<GameObject>();
        foreach (string t in ok)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(t);
            Debug.Log(assetPath);
            if (!assetPath.Contains(".png"))
                assets.Add(AssetDatabase.LoadAssetAtPath<GameObject>(assetPath));
        }

        string folderLocation = "/Prefabs/Export/";
        string dirPath = Application.dataPath + folderLocation;
        if(!Directory.Exists(dirPath))
        {
            Directory.CreateDirectory(dirPath);
        }
        List<string> thumbnailsPaths = new List<string>();
        foreach (GameObject asset in assets)
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
            string imagePath = dirPath + asset.name + ".png";
            thumbnailsPaths.Add("Assets" + folderLocation + asset.name + ".png");
            File.WriteAllBytes(imagePath, bytes);
        }

        AssetDatabase.Refresh();
        foreach (string thumbnailPath in thumbnailsPaths)
        {
            Debug.Log(thumbnailPath);
            AssetDatabase.ImportAsset(thumbnailPath);
            TextureImporter importer = AssetImporter.GetAtPath(thumbnailPath) as TextureImporter;
            importer.textureType = TextureImporterType.Sprite;
            AssetDatabase.WriteImportSettingsIfDirty(thumbnailPath);
        }
        Debug.Log("Thumbnails ok");
    }
    [MenuItem("Assets/Build AssetBundles")]
    static void BuildAllAssetBundles()
    {
        Test();
        string assetBundleDirectory = "Assets/AssetBundles";
        if(!Directory.Exists(assetBundleDirectory))
        {
            Directory.CreateDirectory(assetBundleDirectory);
        }
        #if UNITY_EDITOR_WIN
        BuildTarget target = BuildTarget.StandaloneWindows;
        #endif
        #if UNITY_EDITOR_OSX
        BuildTarget target = BuildTarget.StandaloneOSX;
        #endif
        BuildPipeline.BuildAssetBundles(assetBundleDirectory, 
                                        BuildAssetBundleOptions.None, 
                                        target);
        Debug.Log("Build ok");
    }

}