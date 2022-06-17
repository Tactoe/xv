using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SceneItemSelector : MonoBehaviour
{
    [SerializeField]
    Color m_ButtonColor;
    [SerializeField]
    Color m_ButtonColorHighlighted;
    [SerializeField]
    Color m_TextColor;
    [SerializeField]
    Color m_TextColorHighlighted;
    GameObject m_ItemReference;
    Button m_Button;
    TextMeshProUGUI m_ButtonText;


    public void Init(GameObject i_ItemReference, string i_ItemName)
    {
        m_Button = GetComponent<Button>();
        m_ButtonText = GetComponentInChildren<TextMeshProUGUI>();
        m_ButtonText.text = i_ItemName;
        m_ItemReference = i_ItemReference;
        StartCoroutine(WatchOverReference());
        m_Button.onClick.AddListener(delegate {
            HighlightButton();
            EditWindow.Instance.EnableWindow(m_ItemReference);
        });

    }

    void HighlightButton()
    {
        foreach (Transform sibling in transform.parent)
        {
            sibling.GetComponentInChildren<TextMeshProUGUI>().color = m_TextColor;
            sibling.GetComponent<Image>().color = m_ButtonColor;
        }
        GetComponent<Image>().color = m_ButtonColorHighlighted;
        m_ButtonText.color = m_TextColorHighlighted;
    }

    IEnumerator WatchOverReference()
    {
        while (m_ItemReference != null)
        {
            if (EditWindow.Instance.Target == m_ItemReference)
            {
                HighlightButton();
            }
            else 
            {
                GetComponent<Image>().color = m_ButtonColor;
                m_ButtonText.color = m_TextColor;
            }
            m_ButtonText.text = m_ItemReference.GetComponentInChildren<Item>(true).Data.ItemName;
            yield return new WaitForSeconds(0.1f);
        }
        Destroy(gameObject);
    }

    void OnDisable()
    {
        m_Button.onClick.RemoveAllListeners();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
