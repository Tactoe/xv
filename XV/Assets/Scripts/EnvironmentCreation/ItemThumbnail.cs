using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ItemThumbnail : MonoBehaviour
{
    Button m_Button;
    [SerializeField]
    GameObject m_ThumbnailObject;

    // Start is called before the first frame update
    void Start()
    {
        m_Button = GetComponent<Button>();
        m_Button.onClick.AddListener(delegate {
            ItemHandler.Instance.PlaceMode(m_ThumbnailObject);
        });
    }

    void OnDisable()
    {
        m_Button.onClick.RemoveAllListeners();
    }

}
