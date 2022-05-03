using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillAssetScrollView : MonoBehaviour
{
    [SerializeField]
    GameObject m_Thumbnail;
    [SerializeField]
    GameObject[] m_Assets;
    [SerializeField]
    Transform m_ContentTF;
    // Start is called before the first frame update
    void Start()
    {
        foreach(GameObject asset in m_Assets)
        {
            GameObject itemThumbnail = Instantiate(m_Thumbnail, m_ContentTF);
            itemThumbnail.GetComponent<ItemThumbnail>().ThumbnailObject = asset;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
