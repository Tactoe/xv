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
	private Toggle t_loop;
	private Worker m_scriptWorker;
	void Awake()
	{
		m_scriptWorker = Worker.GetComponent<Worker>();
	}
	void OnEnable()
	{
		t_loop.isOn = m_scriptWorker.Loop;
		UpdateList();
	}
	public void UpdateList()
	{
		if (m_scriptWorker.Tasks.childCount != transform.GetChild(5).GetChild(0).GetChild(0).childCount)
		{
			foreach (Transform child in transform.GetChild(5).GetChild(0).GetChild(0))
			{
				Destroy(child.gameObject);
			}
			foreach (Transform child in m_scriptWorker.Tasks)
			{
				GameObject uitask = Instantiate(TaskPrefab, transform.GetChild(5).GetChild(0).GetChild(0));
				uitask.GetComponent<TaskDestructor>().Task = child.gameObject;
				uitask.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text =
				"#" + child.GetSiblingIndex() + " : " + child.gameObject.name;
			}
			transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Tasks : " + Worker.name;
		}
		if (m_scriptWorker.Loop != t_loop.isOn)
			m_scriptWorker.Loop = t_loop.isOn;
	}
	void FixedUpdate()
	{
		UpdateList();
	}
}
