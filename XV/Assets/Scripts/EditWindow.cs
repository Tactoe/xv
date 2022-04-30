using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EditWindow : MonoBehaviour
{
    public static EditWindow Instance;
    public GameObject Target;
    [SerializeField]
    List<FieldInput> m_Fields;
    [SerializeField]
    TMP_InputField m_NameText;

    void Awake()
    {
        if (Instance != null)
            Destroy(Instance.gameObject);
        Instance = this;
    }
    
    void Start()
    {
        CloseWindow();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Delete) && Target != null
            && ItemHandler.Instance.CheckIfState(EditorState.editItem))
        {
            DestroyObject();
        }
    }

    public void DestroyObject()
    {
        Destroy(Target);
        CloseWindow();
    }

    public void RenameObject(string i_Name)
    {
        Target.name = i_Name;
    }

    public void CloseWindow()
    {
        m_NameText.onValueChanged.RemoveAllListeners();
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
        ItemHandler.Instance.NormalMode();
    }

    public void EnableWindow(GameObject i_Target)
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
        Target = i_Target;
        m_NameText.text = Target.name;
        m_NameText.onValueChanged.AddListener(delegate (string i_Name) {
            Target.name = i_Name;
        });
        m_Fields[0].setFieldValues(Target.transform.localPosition);
        m_Fields[1].setFieldValues(Target.transform.localEulerAngles);
        m_Fields[2].setFieldValues(Target.transform.localScale);
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
}
