using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FillAssetScrollView : MonoBehaviour
{
    [SerializeField]
    GameObject m_CategoryContainer;
    [SerializeField]
    GameObject m_Thumbnail;
    [SerializeField]
    Transform m_ContentTF;
    
    List<GameObject> m_Assets;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Init(List<GameObject> i_Assets, List<Sprite> i_Thumbnails)
    {
        m_Assets = i_Assets;
        Dictionary<string, GameObject> containers = new Dictionary<string, GameObject>();
        foreach(GameObject asset in m_Assets)
        {
            Sprite assetThumbnail = null;
            foreach (Sprite thumbnail in i_Thumbnails)
            {
                if (thumbnail.name == asset.name)
                {
                    assetThumbnail = thumbnail;
                    break;
                }
            }
            List<ItemTags> tags = asset.GetComponentInChildren<Item>().Data.Tags;
            foreach (ItemTags tag in tags)
            {
                string tagName = tag.ToString();
                if (!containers.ContainsKey(tagName))
                {
                    containers.Add(tagName, Instantiate(m_CategoryContainer, m_ContentTF));
                    containers[tagName].GetComponentInChildren<TextMeshProUGUI>().text = tagName;
                }
                Transform anchor = containers[tagName].transform.Find("Content");
                GameObject itemThumbnail = Instantiate(m_Thumbnail, anchor);
                itemThumbnail.GetComponent<AssetThumbnail>().Init(asset, assetThumbnail);
            }
        }
    }
}
