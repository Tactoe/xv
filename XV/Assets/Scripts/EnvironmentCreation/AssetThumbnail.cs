using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class AssetThumbnail : MonoBehaviour
{
    GameObject m_ThumbnailObject;

    [SerializeField]
    TextMeshProUGUI m_Title;
    [SerializeField]
    Image m_Img;
    [SerializeField]
    Button m_Button;

    public void Init(GameObject i_ThumbnailObject, Sprite i_ThumbnailSprite)
    {
        m_ThumbnailObject = i_ThumbnailObject;
        m_Img.sprite = i_ThumbnailSprite;
        m_Title.text = i_ThumbnailObject.name;
        m_Button.onClick.AddListener(delegate {
            ItemHandler.Instance.PlaceMode(m_ThumbnailObject);
        });
    }

    void OnDisable()
    {
        m_Button.onClick.RemoveAllListeners();
    }

}
