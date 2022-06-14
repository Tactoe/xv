using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorBand : MonoBehaviour
{
    [SerializeField]
    GameObject m_ColorPicker;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(delegate {
            CreatePicker();
        });
    }

    void CreatePicker()
    {
        // GameObject previousColorPicker = FindObjectOfType<ColorPicker>()?.gameObject;
        // if (previousColorPicker != null)
        //     Destroy(previousColorPicker);
        // RectTransform editWindowRT = (transform.root.GetChild(0) as RectTransform);
        // GameObject pickerGO = Instantiate(m_ColorPicker, transform.root);
        // ColorPicker picker = pickerGO.GetComponent<ColorPicker>();
        // picker.CurrentColorIndex = int.Parse(gameObject.name);
        // float xPos = (editWindowRT.sizeDelta.x / 2) + editWindowRT.anchoredPosition.x;
        // (pickerGO.transform as RectTransform).anchoredPosition = new Vector2(xPos, editWindowRT.anchoredPosition.y);
        GameObject previousColorPicker = FindObjectOfType<ColorPicker>()?.gameObject;
        if (previousColorPicker != null)
            Destroy(previousColorPicker);
        RectTransform editWindowRT = (transform.parent as RectTransform);
        GameObject pickerGO = Instantiate(m_ColorPicker, transform.parent);
        ColorPicker picker = pickerGO.GetComponent<ColorPicker>();
        picker.CurrentColorIndex = int.Parse(gameObject.name);
        float xPos = (editWindowRT.sizeDelta.x);
        (pickerGO.transform as RectTransform).anchoredPosition = new Vector2(xPos, editWindowRT.anchoredPosition.y);
    }
}
