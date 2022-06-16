using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using TMPro;

[Serializable]
public class SaveDataObject
{
    public ItemData[] itemArray;
	public TaskData[] taskArray;
}

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;
    public string SaveIdAfterSave;

    [SerializeField]
    Transform m_SceneItemListTF;
    [SerializeField]
    GameObject m_SceneItemSelector;
	[SerializeField]
	GameObject m_ScenePrefab;
    [SerializeField]
    GameObject[] m_ObjectList;
	[SerializeField]
	GameObject[] m_TaskList;
    Transform m_SceneAnchor;
    [SerializeField]
    GameObject err_id_not_found;
    
    [SerializeField]
    TMP_InputField MyFieldId;
    // Start is called before the first frame update

    void Awake(){
        Instance = this;
        

    }

    void Start()
    {
        if (SceneManager.GetActiveScene().name != "titre_menu"){
            m_SceneAnchor = GameObject.FindGameObjectWithTag("Scene").transform;
            // Debug.Log("Current_Scene = " + PlayerPrefs.GetString("Current_Scene") );+
            string tmp = PlayerPrefs.GetString("Current_Scene");
            if (tmp != "" ){LoadLocal(tmp);}
        }
    }

    [ContextMenu("Save")]
    public void Save(string i_SaveName, bool i_SaveOnline)
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
        if (i_SaveOnline)
            StartCoroutine(UploadSave(JsonUtility.ToJson(saveData)));
        print(PlayerPrefs.GetString(i_SaveName));
    }

    IEnumerator UploadSave(string i_SaveData)
    {
        WWWForm form = new WWWForm();
        form.AddField("saveContent", i_SaveData);

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost:8080/setSave.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                string responseText = www.downloadHandler.text;
                Debug.Log("Response Text:" + responseText);
                Debug.Log("Form upload complete!");
                SaveIdAfterSave = responseText;
            }
        }
    }

    void SetTaskDataPosition(Transform i_Transform, TaskData i_Data)
    {
        i_Data.Position = i_Transform.localPosition;
    }

    void SetItemDataPosition(Transform i_Transform, ItemData i_Data)
    {
        i_Data.Position = i_Transform.localPosition;
        i_Data.Rotation = i_Transform.localEulerAngles;
        i_Data.Scale = i_Transform.localScale;
    }
    
    public void LoadLocal(string i_SaveName)
    {
		DestroyImmediate(m_SceneAnchor.gameObject);
		m_SceneAnchor = Instantiate(m_ScenePrefab).transform;
		m_SceneAnchor.name = m_ScenePrefab.name;
        print(PlayerPrefs.GetString(i_SaveName));
        Load(PlayerPrefs.GetString(i_SaveName));
    }
    
    [ContextMenu("Load")]
    public void LoadOnline()
    {
        string save_id = MyFieldId.text;
        StartCoroutine(GetOnlineSave(save_id));
    }

    IEnumerator GetOnlineSave(string i_SaveId)
    {
        WWWForm form = new WWWForm();
        form.AddField("saveId", i_SaveId);

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost:8080/getSave.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                string responseText = www.downloadHandler.text;
                Debug.Log("Response Text:" + responseText);

                if(responseText != ""){
                    PlayerPrefs.SetString(i_SaveId, responseText);
                    PlayerPrefs.SetString("Current_Scene", i_SaveId);
                    Time.timeScale = 1;
                    SceneManager.LoadScene("Default");
                }
                else{
                    err_id_not_found.SetActive(false);
                }
            }
        }
    }
    
    public void Load(string i_Save)
    {
        SaveDataObject saveDataObject = JsonUtility.FromJson<SaveDataObject>(i_Save);
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
            GameObject tmp = Instantiate(m_SceneItemSelector, m_SceneItemListTF);
            tmp.GetComponent<SceneItemSelector>().Init(instantiated, item.PrefabName);
        }
		foreach (TaskData taskData in saveDataObject.taskArray)
		{
			GameObject toInstantiate = null;
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
}
