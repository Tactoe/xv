using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class RendererObject
{
    public Material mat;
    public List<Renderer> meshRenderers;
    public List<int> meshRenderMatIndexes;

    public RendererObject(Material i_Mat, Renderer i_MeshRend, int i_MeshRendMatIndex)
    {
        mat = i_Mat;
        meshRenderers = new List<Renderer>();
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
        if (Target && m_TrackMovement)
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

        BuildColorPanel(itemData.ColorOverride);
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
    
    public void BuildColorPanel(List<string> i_ColorOverride)
    {
        foreach (Transform child in m_ColorPanelTF)
        {
            Destroy(child.gameObject);
        }
        m_TargetRenderers?.Clear();
        m_TargetColors?.Clear();
        
        ColorOverrider overrider = Target.AddComponent<ColorOverrider>();
        m_TargetRenderers = overrider.ExtractMats(Target.transform, new List<RendererObject>());
        Destroy(overrider);
        int index = 0;
        // Initialize colorOverride list if it's null
        if (i_ColorOverride.Count == 0)
        {
            i_ColorOverride = new List<string>();
            for (int i = 0; i < m_TargetRenderers.Count; i++)
                i_ColorOverride.Add(null);
        }
        foreach (RendererObject renderer in m_TargetRenderers)
        {
            if (i_ColorOverride[index] != null)
            {
                Color overrideColor = new Color();
                ColorUtility.TryParseHtmlString(i_ColorOverride[index], out overrideColor);
                m_TargetColors.Add(overrideColor);
            }
            else
                m_TargetColors.Add(renderer.mat.color);
            GameObject tmp = Instantiate(m_ColorBand, m_ColorPanelTF);
            tmp.name = index.ToString();
            tmp.GetComponent<Image>().color = m_TargetColors.Last();
            index++;
        }
        Target.GetComponentInChildren<Item>().Data.ColorOverride = i_ColorOverride;
    }

    
    public void ChangeColor(Color i_NewColor, int i_Index)
    {
        int i = i_Index;
        MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
        m_ColorPanelTF.transform.GetChild(i).GetComponent<Image>().color = i_NewColor;
        m_TargetColors[i] = i_NewColor;
        for(int j = 0; j < m_TargetRenderers[i].meshRenderers.Count; j++)
        {
            Renderer meshRenderer = m_TargetRenderers[i].meshRenderers[j];
            int materialIndex = m_TargetRenderers[i].meshRenderMatIndexes[j];
            meshRenderer.GetPropertyBlock(propertyBlock);
            propertyBlock.SetColor("_Color", m_TargetColors[i]);
            meshRenderer.SetPropertyBlock(propertyBlock, materialIndex);
        }
        Target.GetComponentInChildren<Item>().Data.ColorOverride[i] = "#" + ColorUtility.ToHtmlStringRGB(i_NewColor);
    }
}