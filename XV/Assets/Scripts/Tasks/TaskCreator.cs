using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskCreator : MonoBehaviour
{
	[SerializeField]
	private GameObject m_taskPrefab;
	private Camera m_camera;
	static private GameObject m_tmpTask;
	static private Timeline m_timeline;
	static public bool Hovering;
	void Awake()
	{
		m_camera = GameObject.Find("Main Camera").GetComponent<Camera>();
		m_timeline = GameObject.Find("Timeline").GetComponent<Timeline>();
	}
	void Start()
	{
	}
	public void NewTask()
	{
		if(!m_timeline.Running)
		{
			if (m_tmpTask)
				Cancel();
			m_tmpTask =
			Instantiate(m_taskPrefab, transform.parent.parent.GetComponent<TaskList>().Worker.transform.GetChild(1));
			m_tmpTask.name = m_taskPrefab.name;
		}
	}
	void Create()
	{
		m_tmpTask = null;
	}
	void Cancel()
	{
		if (m_tmpTask)
		{
			Destroy(m_tmpTask);
			m_tmpTask = null;
		}
	}
	public void SetHovering(bool i)
	{
		Hovering = i;
		if (!m_tmpTask)
			Hovering = false;
	}
	void Update()
	{
		if (m_tmpTask)
		{
			if (m_timeline.Running)
			{
				Cancel();
				return ;
			}
			RaycastHit hit;
			Ray ray = m_camera.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit))
				m_tmpTask.transform.position = hit.point;
			if (Input.GetMouseButton(0) && !Hovering)
				Create();
			else if (Input.GetMouseButton(1))
				Cancel();
		}
	}
}
