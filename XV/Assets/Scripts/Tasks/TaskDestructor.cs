using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskDestructor : MonoBehaviour
{
	public GameObject Task;
	private TaskList m_taskList;
	void Awake()
	{
		m_taskList = GameObject.Find("TaskList").GetComponent<TaskList>();
	}
	public void Destroy()
	{
		Destroy(Task);
	}
}
