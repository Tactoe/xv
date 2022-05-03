using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillAssetScrollView : MonoBehaviour
{
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
            GameObject itemThumbnail = Instantiate(m_Thumbnail, m_ContentTF);
            itemThumbnail.GetComponent<AssetThumbnail>().Init(asset, assetThumbnail);
            //itemThumbnail.GetComponent<ItemThumbnail>().GenerateThumbnails(asset);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
