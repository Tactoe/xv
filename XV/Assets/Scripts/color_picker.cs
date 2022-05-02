using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class color_picker : MonoBehaviour
{
    public Image my_demo_square;
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
        Color old_color = my_demo_square.color;
        my_demo_square.color = new Color(newValue, old_color.g, old_color.b, 1);
        Debug.Log("my colo = " + my_demo_square.color);
    }
    
    public void OnValueChanged_green(float newValue)
    {
        Debug.Log("{GREE} ma value = " + newValue);
        Color old_color = my_demo_square.color;
        my_demo_square.color = new Color(old_color.r, newValue, old_color.b, 1);
        Debug.Log("my colo = " + my_demo_square.color);
        
    }
    
    public void OnValueChanged_blu(float newValue)
    {
        Debug.Log("{BLU} ma value = " + newValue);
        Color old_color = my_demo_square.color;
        my_demo_square.color = new Color(old_color.r, old_color.g, newValue, 1);
        Debug.Log("my colo = " + my_demo_square.color);
        
    }

    public Color OK(){
        //return my_demo_square.color
    }
}

