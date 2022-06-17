using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

enum fieldType {
    transform, rotate, scale
}

public class FieldInput : MonoBehaviour
{
    [SerializeField]
    TMP_InputField[] m_Fields;
    [SerializeField]
    EditWindow m_Window;
    [SerializeField]
    fieldType m_FieldType;
    
    Vector3 m_Value;
    
    void Start()
    {
        m_Fields[0].onValueChanged.AddListener(delegate {ValueChanged(0);});
        m_Fields[1].onValueChanged.AddListener(delegate {ValueChanged(1);});
        m_Fields[2].onValueChanged.AddListener(delegate {ValueChanged(2);});
    }

    public void setFieldValues(Vector3 newValues)
    {
        float roundDecimal = 1000f;
        for (int i = 0; i < 3; i++)
        {
            newValues[i] = Mathf.Round(newValues[i] * roundDecimal) / roundDecimal;
            m_Fields[i].text = newValues[i].ToString();
        }
    }

    public void ValueChanged(int index)
    {
        float newValue;
        if (m_Fields[index].text == "")
            newValue = 0;
        else if (m_Fields[index].text == "-")
        {
            m_Fields[index].text = "-0";
            newValue = 0;
        }
        else
            if (!float.TryParse(m_Fields[index].text, out newValue))
                return;
        newValue = Mathf.Clamp(newValue, -1000000000, 1000000000);
        m_Value[index] = newValue;
        
        switch (m_FieldType)
        {
            case fieldType.transform:
                m_Window.ChangeTargetPosition(m_Value);
                break;
            case fieldType.rotate:
                m_Window.ChangeTargetRotation(m_Value);
                break;
            case fieldType.scale:
                m_Window.ChangeTargetScale(m_Value);
                break;
        }

    }
}
