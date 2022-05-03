using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskDestructor : MonoBehaviour
{
	public GameObject Task;
	private TaskList m_TaskList;
	void Awake()
	{
		m_TaskList = GameObject.Find("TaskList").GetComponent<TaskList>();
	}
	public void Destroy()
	{
		Destroy(Task);
	}
}
