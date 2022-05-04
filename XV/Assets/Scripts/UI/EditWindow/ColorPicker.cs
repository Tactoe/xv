using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorPicker : MonoBehaviour
{
    public Image DemoSquare;
    public int CurrentColorIndex;
    public Slider[] Sliders;

    void Start()
    {
        List<string> colorOverride = EditWindow.Instance.Target.GetComponentInChildren<Item>().Data.ColorOverride;
        string overrideString = colorOverride[CurrentColorIndex];
        if (overrideString != null)
        {
            Color overrideColor = new Color();
            ColorUtility.TryParseHtmlString(overrideString, out overrideColor);
            DemoSquare.color = overrideColor;
            Sliders[0].value = overrideColor.r;
            Sliders[1].value = overrideColor.g;
            Sliders[2].value = overrideColor.b;
        }
        for (int i = 0; i < 3; i++)
        {
            Sliders[i].onValueChanged.AddListener(delegate (float value) {
                OnValueChanged();
            });
        }
    }

    public void OnValueChanged()
    {
        DemoSquare.color = new Color(Sliders[0].value, Sliders[1].value, Sliders[2].value, 1);
        EditWindow.Instance.ChangeColor(DemoSquare.color, CurrentColorIndex);
    }

    public void OK(){
        Destroy(gameObject);
    }
}

