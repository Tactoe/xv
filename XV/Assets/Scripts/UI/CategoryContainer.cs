using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CategoryContainer : MonoBehaviour
{
    RectTransform m_RT;
    ContentSizeFitter m_ContentSizeFitter;
    [SerializeField]
    RectTransform m_LayoutGroup;
    bool m_Open = false;
    // Start is called before the first frame update
    void Start()
    {
        m_RT = GetComponent<RectTransform>();   
        m_LayoutGroup = GetComponent<RectTransform>();
        m_ContentSizeFitter = GetComponent<ContentSizeFitter>();   
    }

    public void ToggleContainer()
    {
        m_Open = !m_Open;
        if (m_Open)
        {
            m_ContentSizeFitter.enabled = true;
        }
        else
        {
            m_ContentSizeFitter.enabled = false;
            m_RT.sizeDelta = new Vector2(m_RT.sizeDelta.x, 64);
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(m_LayoutGroup);
    }
}
