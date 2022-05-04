using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;

public class TaskList : MonoBehaviour
{
	public GameObject Worker;
	public GameObject TaskPrefab;
	[SerializeField]
	private GameObject m_Content;
	[SerializeField]
	private Toggle m_Loop;
	private EditWindow m_Target;
	private Worker m_ScriptWorker;
	private Transform m_TaskList;
	void Awake()
	{
		m_Loop.isOn = false;
		// m_TaskList = transform.GetChild(0).GetChild(6).GetChild(0).GetChild(0);
		m_TaskList = GameObject.Find("Content2").transform;
		m_Target = GameObject.Find("ItemWindow").GetComponent<EditWindow>();
		m_Content.SetActive(false);
	}

	public void SwitchList()
	{
		m_Content.SetActive(!m_Content.activeInHierarchy);
		if (m_Content.activeInHierarchy)
			UpdateList();
	}

	public void UpdateList(bool i_idxChanged = false)
	{
		Worker = m_Target.Target;
		m_ScriptWorker = Worker.GetComponent<Worker>();
		if (m_ScriptWorker.Tasks.childCount != m_TaskList.childCount || i_idxChanged)
		{
			foreach (Transform child in m_TaskList)
			{
				Destroy(child.gameObject);
			}
			foreach (Transform child in m_ScriptWorker.Tasks)
			{
				GameObject uitask = Instantiate(TaskPrefab, m_TaskList);
				uitask.GetComponent<TaskDestructor>().Task = child.gameObject;
				uitask.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text =
				"#" + child.GetSiblingIndex() + " : " + child.gameObject.name;
				uitask.transform.Find("Wait").GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text =
				child.GetComponent<Task>().Wait.ToString();
			}
			transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Tasks : " + Worker.name;
		}
		if (m_ScriptWorker.Loop != m_Loop.isOn)
			m_ScriptWorker.Loop = m_Loop.isOn;
	}

	void FixedUpdate()
	{
		if (m_Target.Target && m_Target.Target.CompareTag("Worker"))
			UpdateList();
	}
}
