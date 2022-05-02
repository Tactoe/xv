using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class color_picker : MonoBehaviour
{
    public Image myDemoSquare;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void OnValueChanged_red(float newValue)
    {
        Debug.Log("{RED} ma value = " + newValue);
        Color oldColor = myDemoSquare.color;
        myDemoSquare.color = new Color(newValue, oldColor.g, oldColor.b, 1);
        Debug.Log("my colo = " + myDemoSquare.color);
    }
    
    public void OnValueChanged_green(float newValue)
    {
        Debug.Log("{GREE} ma value = " + newValue);
        Color oldColor = myDemoSquare.color;
        myDemoSquare.color = new Color(oldColor.r, newValue, oldColor.b, 1);
        Debug.Log("my colo = " + myDemoSquare.color);
        
    }
    
    public void OnValueChanged_blu(float newValue)
    {
        Debug.Log("{BLU} ma value = " + newValue);
        Color oldColor = myDemoSquare.color;
        myDemoSquare.color = new Color(oldColor.r, oldColor.g, newValue, 1);
        Debug.Log("my colo = " + myDemoSquare.color);
        
    }

    public Color OK(){
        return myDemoSquare.color;
    }
}

