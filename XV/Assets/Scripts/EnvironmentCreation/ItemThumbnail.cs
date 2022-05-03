using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.IO;

[RequireComponent(typeof(Button))]
public class ItemThumbnail : MonoBehaviour
{
    [SerializeField]
    Sprite[] m_ThumbnailSprites;
    GameObject m_ThumbnailObject;

    Image m_Img;
    Button m_Button;

    public void Init(GameObject i_ThumbnailObject)
    {
        m_Img = GetComponent<Image>();
        m_ThumbnailObject = i_ThumbnailObject;
        foreach (Sprite spr in m_ThumbnailSprites)
        {
            if (spr.name == i_ThumbnailObject.name)
            {
                m_Img.sprite = spr;//AssetPreview.GetAssetPreview(m_ThumbnailObject);
                break;
            }
        }
        m_Button = GetComponent<Button>();
        m_Button.onClick.AddListener(delegate {
            ItemHandler.Instance.PlaceMode(m_ThumbnailObject);
        });
    }

    public void GenerateThumbnails(GameObject i_ThumbnailObject)
    {
        Texture2D texture = AssetPreview.GetAssetPreview(i_ThumbnailObject);
        byte[] bytes = texture.EncodeToPNG();
        print(Application.dataPath);
        var dirPath = Application.dataPath + "/../SaveImages/";
                  if(!Directory.Exists(dirPath)) {
                      Directory.CreateDirectory(dirPath);
                  }
                  File.WriteAllBytes(dirPath + i_ThumbnailObject.name + ".png", bytes);
    }

    void OnDisable()
    {
        m_Button.onClick.RemoveAllListeners();
    }

}
