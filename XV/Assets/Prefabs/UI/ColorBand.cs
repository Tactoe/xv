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
        GameObject previousColorPicker = FindObjectOfType<ColorPicker>()?.gameObject;
        if (previousColorPicker != null)
            Destroy(previousColorPicker);
        GameObject pickerGO = Instantiate(m_ColorPicker, transform.parent);
        ColorPicker picker = pickerGO.GetComponent<ColorPicker>();
        picker.CurrentColorIndex = int.Parse(gameObject.name);
        float xPos = ((transform as RectTransform).sizeDelta.x / 2) + (transform as RectTransform).anchoredPosition.x;
        (pickerGO.transform as RectTransform).anchoredPosition = new Vector2(xPos, (transform as RectTransform).anchoredPosition.y);
    }
}
