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
	void Awake()
	{
		m_TaskList = GameObject.Find("TaskList").GetComponent<TaskList>();
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
	public void Destroy()
	{
		Destroy(Task);
	}
}
