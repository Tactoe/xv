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
        print(ToSerialize);
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
        SaveDataObject saveDataObject = JsonUtility.FromJson<SaveDataObject>(PlayerPrefs.GetString("Save"));
        print(PlayerPrefs.GetString("Save"));
        ItemData[] ToBuild = saveDataObject.Array;
        print(ToBuild);
        foreach(ItemData item in ToBuild)
        {
            GameObject toInstantiate = null;
            foreach (GameObject obj in m_ObjectList)
            {
                if (item.PrefabName == obj.name)
                {
                    toInstantiate = obj;
                }
            }
            print("Instantiating stuff");
            GameObject instantiated = Instantiate(toInstantiate, m_SceneAnchor.transform);
            ItemData instantiatedItemData = instantiated.GetComponentInChildren<Item>().Data;
            instantiatedItemData = item;
            instantiated.transform.position = item.Position;
            instantiated.transform.localEulerAngles = item.Rotation;
            instantiated.transform.localScale = item.Scale;

        }
    }

    List<GameObject> BuildGameObjectList(Transform i_Anchor, List<GameObject> i_GOList)
    {
        foreach(Transform child in i_Anchor)
        {
            if (!child.CompareTag("Ground"))
                i_GOList.Add(child.gameObject);
            if (child.childCount > 0)
            {
                BuildGameObjectList(child, i_GOList);
            }
        }
        return i_GOList;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
