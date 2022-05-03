using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ItemThumbnail : MonoBehaviour
{
    public GameObject ThumbnailObject;
    
    Button m_Button;

    void Start()
    {
        m_Button = GetComponent<Button>();
        m_Button.onClick.AddListener(delegate {
            ItemHandler.Instance.PlaceMode(ThumbnailObject);
        });
    }

    void OnDisable()
    {
        m_Button.onClick.RemoveAllListeners();
    }

}
