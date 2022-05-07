using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


[Serializable]
public class SaveDataObject
{
    public ItemData[] itemArray;
	public TaskData[] taskArray;
}

public class SaveManager : MonoBehaviour
{
	[SerializeField]
	GameObject m_ScenePrefab;
    public static SaveManager Instance;
    void Awake(){
        Instance = this;
        

    }

    [SerializeField]
    GameObject[] m_ObjectList;
	[SerializeField]
	GameObject[] m_TaskList;
    Transform m_SceneAnchor;
    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().name != "titre_menu"){
            m_SceneAnchor = GameObject.FindGameObjectWithTag("Scene").transform;
            // Debug.Log("Current_Scene = " + PlayerPrefs.GetString("Current_Scene") );+
            string tmp = PlayerPrefs.GetString("Current_Scene");
            if (tmp != "" ){Load(tmp);}
        }
    }

    [ContextMenu("Save")]
    public void Save(string i_SaveName)
    {
        List<ItemData> itemToSerialize =  new List<ItemData>();
		List<TaskData> taskToSerialize =  new List<TaskData>();
        foreach(Transform child in m_SceneAnchor.transform)
        {
            if (child.CompareTag("Ground"))
                continue;
            ItemData itemData = child.GetComponentInChildren<Item>().Data;
            SetItemDataPosition(child, itemData);
            itemToSerialize.Add(itemData);
        }
		foreach(Transform child in m_SceneAnchor.transform)
        {
            if (!child.CompareTag("Worker"))
                continue;
            Transform taskAnchor = child.Find("Tasks");
			foreach (Transform taskTF in taskAnchor)
			{
				TaskData taskData = taskTF.GetComponentInChildren<Task>().Data;
            	taskData.Position = taskTF.localPosition;
				taskData.PrefabName = taskTF.name;
            	taskToSerialize.Add(taskData);
			}
        }
		
		
        SaveDataObject saveData = new SaveDataObject();
        saveData.itemArray = itemToSerialize.ToArray();
		saveData.taskArray = taskToSerialize.ToArray();
        PlayerPrefs.SetString(i_SaveName, JsonUtility.ToJson(saveData));
        print(PlayerPrefs.GetString(i_SaveName));
    }

    void SetTaskDataPosition(Transform i_Transform, TaskData ok)
    {
        ok.Position = i_Transform.localPosition;
    }

    void SetItemDataPosition(Transform i_Transform, ItemData ok)
    {
        ok.Position = i_Transform.localPosition;
        ok.Rotation = i_Transform.localEulerAngles;
        ok.Scale = i_Transform.localScale;
    }
    
    [ContextMenu("Load")]
    public void Load(string i_SaveName)
    {
		DestroyImmediate(m_SceneAnchor.gameObject);
		m_SceneAnchor = Instantiate(m_ScenePrefab).transform;
		m_SceneAnchor.name = m_ScenePrefab.name;
        print(PlayerPrefs.GetString(i_SaveName));
        SaveDataObject saveDataObject = JsonUtility.FromJson<SaveDataObject>(PlayerPrefs.GetString(i_SaveName));
        foreach (ItemData item in saveDataObject.itemArray)
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
            Debug.Log(toInstantiate);
            Debug.Log(m_SceneAnchor.transform);
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
			print("Instantiated " + instantiated.name + " With index = " + instantiated.transform.GetSiblingIndex());
        }
		foreach (TaskData taskData in saveDataObject.taskArray)
		{
			GameObject toInstantiate = null;
			print("Looking for " + taskData.RelatedWorkerID);
			Transform workerTF = m_SceneAnchor.GetChild(taskData.RelatedWorkerID);
			Transform taskAnchor = workerTF.Find("Tasks");
			foreach (GameObject task in m_TaskList)
            {
                string prefabName = task.name;
                if (prefabName == taskData.PrefabName)
                {
                    toInstantiate = task;
                }
            }
            if (toInstantiate == null)
            {
                Debug.Log("Saved object was not found in prefab list");
                continue;
            }
			GameObject instantiated = Instantiate(toInstantiate, taskAnchor);
			instantiated.name = taskData.PrefabName;
			if (taskData.HasParent)
			{
				GameObject interactable = m_SceneAnchor.GetChild(taskData.RelatedParentID).gameObject;
				instantiated.GetComponent<Task>().SetInteractable(interactable);
			}
			else
				instantiated.transform.localPosition = taskData.Position;
			

		}
    }

    // Update is called once per frame
    // void Update()
    // {
        
    //     if (Input.GetKeyDown(KeyCode.Alpha1))
    //     {
    //         Save();
    //     }
    //     if (Input.GetKeyDown(KeyCode.Alpha2))
    //     {
    //         Load();
    //     }
    // }
}
