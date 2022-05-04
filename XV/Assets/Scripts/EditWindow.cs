using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RendererObject
{
    public Material mat;
    public List<MeshRenderer> meshRenderers;
    public List<int> meshRenderMatIndexes;

    public RendererObject(Material i_Mat, MeshRenderer i_MeshRend, int i_MeshRendMatIndex)
    {
        mat = i_Mat;
        meshRenderers = new List<MeshRenderer>();
        meshRenderMatIndexes = new List<int>();
        meshRenderers.Add(i_MeshRend);
        meshRenderMatIndexes.Add(i_MeshRendMatIndex);
    }
}

public class EditWindow : MonoBehaviour
{
    public static EditWindow Instance;
    public GameObject Target;
    
    
    [SerializeField]
    GameObject m_TaskButton;
    [SerializeField]
    GameObject m_ColorPicker;
    [SerializeField]
    Transform m_ColorPanelTF;
    [SerializeField]
    GameObject m_ColorBand;
    [SerializeField]
    List<FieldInput> m_Fields;
    [SerializeField]
    TMP_InputField m_NameText;
    [SerializeField]
    List<Color> m_TargetColors;
    [SerializeField]
    List<RendererObject> m_TargetRenderers;

    bool m_TrackMovement;

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
        if (m_TrackMovement)
        {
            m_Fields[0].setFieldValues(Target.transform.localPosition);
            m_Fields[1].setFieldValues(Target.transform.localEulerAngles);
            m_Fields[2].setFieldValues(Target.transform.localScale);
        }
    }
    
    void Movement(EditorState i_State)
    {
            m_TrackMovement = i_State == EditorState.placingItem;
    }

    // ########### WINDOW TOGGLE ##################

    public void EnableWindow(GameObject i_Target)
    {
        ItemHandler.ModeChanged += Movement;
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
        Target = i_Target;
        ItemData itemData = Target.GetComponentInChildren<Item>().Data;
        print(itemData.ItemName);
        m_NameText.text = itemData.ItemName;
        m_NameText.onEndEdit.AddListener(delegate (string i_Name) {
            Target.GetComponentInChildren<Item>().Data.ItemName = i_Name;
        });
        m_Fields[0].setFieldValues(Target.transform.localPosition);
        m_Fields[1].setFieldValues(Target.transform.localEulerAngles);
        m_Fields[2].setFieldValues(Target.transform.localScale);

		if (Target.CompareTag("Worker"))
			m_TaskButton.SetActive(true);
		else
			m_TaskButton.SetActive(false);

        GetColor();
    }

    public void CloseWindow()
    {
        ItemHandler.ModeChanged -= Movement;
        m_NameText.onEndEdit.RemoveAllListeners();
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
        m_TrackMovement = false;
        ItemHandler.Instance.NormalMode();
    }

    // ########### TRANSFORM CHANGE ##################

    public void MoveObjectAgain()
    {
        m_TrackMovement = true;
        ItemHandler.Instance.PlaceMode(Target, false);
    }

    public void ChangeTargetPosition(Vector3 i_NewPos)
    {
        Target.transform.localPosition = i_NewPos;
    }
    
    public void ChangeTargetRotation(Vector3 i_NewRot)
    {
        Target.transform.localRotation =  Quaternion.Euler(i_NewRot);
    }
    public void ChangeTargetScale(Vector3 i_NewScale)
    {
        Target.transform.localScale = i_NewScale;
    }

    public void DestroyObject()
    {
        Destroy(Target);
        CloseWindow();
    }

    // ########### COLOR CHANGE ##################
    
    List<RendererObject> ExtractMats(Transform target, List<RendererObject> i_MaterialList)
    {
        foreach (Transform child in target)
        {
            MeshRenderer mesh = child.GetComponent<MeshRenderer>();
            if (mesh != null)
            {
                for (int i = 0; i < mesh.sharedMaterials.Length; i++)
                {
                    if (!mesh.sharedMaterials[i].shader.name.Contains("Multiple"))
                        continue;
                    bool isNewMat = true;
                    for(int j = 0; j < i_MaterialList.Count; j++)
                    {
                        if (i_MaterialList[j].mat.color == mesh.sharedMaterials[i].color
                            && i_MaterialList[j].mat.name == mesh.sharedMaterials[i].name)
                        {
                            i_MaterialList[j].meshRenderers.Add(mesh);
                            i_MaterialList[j].meshRenderMatIndexes.Add(i);
                            isNewMat = false;
                            break;
                        }
                    }
                    if (isNewMat)
                    {
                        RendererObject tmp = new RendererObject(mesh.sharedMaterials[i], mesh, i);
                        i_MaterialList.Add(tmp);
                    }
                }
            }
            ExtractMats(child, i_MaterialList);
        }
        return i_MaterialList;
    }

    public void GetColor()
    {
        foreach (Transform child in m_ColorPanelTF)
        {
            Destroy(child.gameObject);
        }
        m_TargetRenderers?.Clear();
        m_TargetColors?.Clear();
        m_TargetRenderers = ExtractMats(Target.transform, new List<RendererObject>());
        int index = 0;
        foreach (RendererObject renderer in m_TargetRenderers)
        {
            m_TargetColors.Add(renderer.mat.color);
            GameObject tmp = Instantiate(m_ColorBand, m_ColorPanelTF);
            tmp.name = index.ToString();
            tmp.GetComponent<Image>().color = renderer.mat.color;
            index++;
        }
    }

    
    public void ChangeColor(Color i_NewColor, int i_Index)
    {
        int i = i_Index;
        MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
        m_ColorPanelTF.transform.GetChild(i).GetComponent<Image>().color = i_NewColor;
        m_TargetColors[i] = i_NewColor;
        for(int j = 0; j < m_TargetRenderers[i].meshRenderers.Count; j++)
        {
            MeshRenderer meshRenderer = m_TargetRenderers[i].meshRenderers[j];
            int materialIndex = m_TargetRenderers[i].meshRenderMatIndexes[j];
            meshRenderer.GetPropertyBlock(propertyBlock);
            propertyBlock.SetColor("_Color", m_TargetColors[i]);
            meshRenderer.SetPropertyBlock(propertyBlock, materialIndex);
        }
    }
}
