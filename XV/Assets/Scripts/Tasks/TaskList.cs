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
		Debug.Log("m_task = " + m_TaskList);
		m_Target = GameObject.Find("ItemWindow").GetComponent<EditWindow>();
		m_Content.SetActive(false);
	}

	public void SwitchList()
	{
		m_Content.SetActive(!m_Content.activeInHierarchy);
		if (m_Content.activeInHierarchy)
			UpdateList();
	}

	public void UpdateList()
	{
		Worker = m_Target.Target;
		m_ScriptWorker = Worker.GetComponent<Worker>();
		if (m_ScriptWorker.Tasks.childCount != m_TaskList.childCount)
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
			}
			transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Tasks : " + Worker.name;
		}
		if (m_ScriptWorker.Loop != m_Loop.isOn)
			m_ScriptWorker.Loop = m_Loop.isOn;
	}

	void FixedUpdate()
	{
		if (m_Target.Target)
			UpdateList();
	}
}
