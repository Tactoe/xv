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
    TMP_InputField[] fields;
    [SerializeField]
    EditWindow m_window;
    [SerializeField]
    fieldType m_fieldType;
    Vector3 value;
    // Start is called before the first frame update
    void Start()
    {
        fields[0].onValueChanged.AddListener(delegate {ValueChanged(0);});
        fields[1].onValueChanged.AddListener(delegate {ValueChanged(1);});
        fields[2].onValueChanged.AddListener(delegate {ValueChanged(2);});
    }

    void setFieldValues(Vector3 newValues)
    {
        for (int i = 0; i < 3; i++)
        {
            fields[i].text = newValues[i].ToString();
        }
    }

    public void ValueChanged(int index)
    {
        float newValue;
        if (fields[index].text == "" || fields[index].text == "-")
            newValue = 0;
        else
            newValue = float.Parse(fields[index].text);
        value[index] = newValue;
        
        switch (m_fieldType)
        {
            case fieldType.transform:
                m_window.ChangeTargetPosition(value);
                break;
            case fieldType.rotate:
                m_window.ChangeTargetRotation(value);
                break;
            case fieldType.scale:
                m_window.ChangeTargetScale(value);
                break;
        }

    }
}
