using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditWindow : MonoBehaviour
{
    public static EditWindow Instance;
    public GameObject Target;
    [SerializeField]
    List<FieldInput> m_Fields;

    void Awake()
    {
        if (Instance != null)
            Destroy(Instance.gameObject);
        Instance = this;
    }

    public void ChangeTargetPosition(Vector3 i_NewPos)
    {
        Target.transform.position = i_NewPos;
    }
    
    public void ChangeTargetRotation(Vector3 i_NewRot)
    {
        Target.transform.rotation =  Quaternion.Euler(i_NewRot);
    }
    public void ChangeTargetScale(Vector3 i_NewScale)
    {
        Target.transform.localScale = i_NewScale;
    }
    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
        
    }

    public void EnableWindow(GameObject i_Target)
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
        Target = i_Target;
        m_Fields[0].setFieldValues(i_Target.transform.localPosition);
        m_Fields[1].setFieldValues(i_Target.transform.localEulerAngles);
        m_Fields[2].setFieldValues(i_Target.transform.localScale);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
