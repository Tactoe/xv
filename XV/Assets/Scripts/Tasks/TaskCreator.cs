using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskCreator : MonoBehaviour
{
	[SerializeField]
	private GameObject m_TaskPrefab;
	private Camera m_Camera;
	static private GameObject m_TmpTask;
	static private Timeline m_Timeline;
	static public bool Hovering;
	void Awake()
	{
		m_Camera = GameObject.Find("Main Camera").GetComponent<Camera>();
		m_Timeline = GameObject.Find("Timeline").GetComponent<Timeline>();
	}
	void Start()
	{
	}
	public void NewTask()
	{
		if(!m_Timeline.Running)
		{
			if (m_TmpTask)
				Cancel();
			m_TmpTask =
			Instantiate(m_TaskPrefab, transform.parent.parent.parent.GetComponent<TaskList>().Worker.transform.GetChild(1));
			m_TmpTask.name = m_TaskPrefab.name;
		}
	}
	void Create()
	{
		m_TmpTask = null;
	}
	void Cancel()
	{
		if (m_TmpTask)
		{
			Destroy(m_TmpTask);
			m_TmpTask = null;
		}
	}
	public void SetHovering(bool i)
	{
		Hovering = i;
		if (!m_TmpTask)
			Hovering = false;
	}
	void Update()
	{
		if (m_TmpTask)
		{
			if (m_Timeline.Running)
			{
				Cancel();
				return ;
			}
			RaycastHit hit;
			Ray ray = m_Camera.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit))
				m_TmpTask.transform.position = hit.point;
			if (Input.GetMouseButton(0) && !Hovering)
				Create();
			else if (Input.GetMouseButton(1))
				Cancel();
		}
	}
}
