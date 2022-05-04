using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorPicker : MonoBehaviour
{
    public Image DemoSquare;
    public int CurrentColorIndex;

    public void OnValueChanged_red(float newValue)
    {
        Color oldColor = DemoSquare.color;
        DemoSquare.color = new Color(newValue, oldColor.g, oldColor.b, 1);
        EditWindow.Instance.ChangeColor(DemoSquare.color, CurrentColorIndex);
    }
    
    public void OnValueChanged_green(float newValue)
    {
        Color oldColor = DemoSquare.color;
        DemoSquare.color = new Color(oldColor.r, newValue, oldColor.b, 1);
        EditWindow.Instance.ChangeColor(DemoSquare.color, CurrentColorIndex);
        
    }
    
    public void OnValueChanged_blu(float newValue)
    {
        Color oldColor = DemoSquare.color;
        DemoSquare.color = new Color(oldColor.r, oldColor.g, newValue, 1);
        EditWindow.Instance.ChangeColor(DemoSquare.color, CurrentColorIndex);
        
    }

    public void OK(){
        Destroy(gameObject);
    }
}

