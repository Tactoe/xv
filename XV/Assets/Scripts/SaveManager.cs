using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveDataObject
{
    public ItemData[] Array;
}

public class SaveManager : MonoBehaviour
{
    [SerializeField]
    GameObject[] m_ObjectList;
    Transform m_SceneAnchor;
    // Start is called before the first frame update
    void Start()
    {
        m_SceneAnchor = GameObject.FindGameObjectWithTag("Scene").transform;
    }

    [ContextMenu("Save")]
    void Save()
    {
        List<ItemData> ToSerialize =  new List<ItemData>();
        foreach(Transform child in m_SceneAnchor.transform)
        {
            if (child.CompareTag("Ground"))
                continue;
            ItemData itemData = child.GetComponentInChildren<Item>().Data;
            SetItemDataPosition(child, itemData);
            ToSerialize.Add(itemData);
        }
        SaveDataObject saveData = new SaveDataObject();
        saveData.Array = ToSerialize.ToArray();
        PlayerPrefs.SetString("Save", JsonUtility.ToJson(saveData));
        print(PlayerPrefs.GetString("Save"));
    }

    void SetItemDataPosition(Transform i_Transform, ItemData ok)
    {
        ok.Position = i_Transform.localPosition;
        ok.Rotation = i_Transform.localEulerAngles;
        ok.Scale = i_Transform.localScale;
    }
    
    [ContextMenu("Load")]
    void Load()
    {
        print(PlayerPrefs.GetString("Save"));
        SaveDataObject saveDataObject = JsonUtility.FromJson<SaveDataObject>(PlayerPrefs.GetString("Save"));
        ItemData[] ToBuild = saveDataObject.Array;
        foreach(ItemData item in ToBuild)
        {
            GameObject toInstantiate = null;
            foreach (GameObject obj in m_ObjectList)
            {
                string prefabName = item.PrefabName.Replace("(Clone)", "");
                if (prefabName == obj.name)
                {
                    toInstantiate = obj;
                }
            }
            if (toInstantiate == null)
            {
                Debug.Log("Saved object was not found in prefab list");
                continue;
            }
            GameObject instantiated = Instantiate(toInstantiate, m_SceneAnchor.transform);
            instantiated.GetComponentInChildren<Item>().Data = item;
            instantiated.name = item.PrefabName;
            instantiated.transform.position = item.Position;
            instantiated.transform.localEulerAngles = item.Rotation;
            instantiated.transform.localScale = item.Scale;
            if (item.ColorOverride.Count > 0)
            {
                ColorOverrider colorOverrider = instantiated.AddComponent<ColorOverrider>();
                colorOverrider.ApplyColorOverride(item.ColorOverride);
                Destroy(colorOverrider);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Save();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Load();
        }
    }
}
