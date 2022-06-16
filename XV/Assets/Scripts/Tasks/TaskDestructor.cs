using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TaskDestructor : MonoBehaviour
{
	public GameObject Task;
	[SerializeField]
	private TextMeshProUGUI m_Wait;
	private TaskList m_TaskList;
	private GameObject m_DescInput;

	// This script handles the logic of vizualisation of tasks into the TaskList
	// The script names come from the first feature of the task list, wich was to destroy tasks
	// It can now handle other things, like reordering the list, give description, and wait timer

	void Awake()
	{
		m_TaskList = GameObject.Find("TaskList").GetComponent<TaskList>();
		m_DescInput = GameObject.Find("TaskList").GetComponent<TaskList>().DescInput;
	}
	public void ChangeWait(string i_Wait)
	{
		int value = int.Parse(i_Wait);
		Task.GetComponent<Task>().Wait = value;
	}

	public void ChangePos(bool i_Up)
	{
		int index = Task.transform.GetSiblingIndex();
		int nb = Task.transform.parent.childCount;
		if (i_Up && index > 0)
			Task.transform.SetSiblingIndex(index - 1);
		else if (!i_Up && index <= nb)
			Task.transform.SetSiblingIndex(index + 1);
			transform.parent.parent.parent.parent.parent.gameObject.GetComponent<TaskList>().UpdateList(true);
	}

	public void SetDescription()
	{
		m_DescInput.SetActive(true);
		m_DescInput.GetComponent<TMP_InputField>().text = Task.GetComponent<Task>().Data.TaskDescription;
		m_TaskList.CurrentDesc = Task.GetComponent<Task>().Data;
	}

	public void Destroy()
	{
		Destroy(Task);
	}
}
