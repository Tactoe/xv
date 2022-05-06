using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskCreator : MonoBehaviour
{
	[SerializeField]
	private GameObject m_TaskPrefab;
	private Camera m_Camera;
	static private GameObject m_TmpTask;
	[SerializeField]
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
			Instantiate(m_TaskPrefab, transform.parent.parent.parent.GetComponent<TaskList>().Worker.transform.Find("Tasks"));
			ItemHandler.Instance.PlaceOrderMode();
			m_TmpTask.name = m_TaskPrefab.name;
		}
	}

	void ReleaseOrder()
	{
		m_TmpTask = null;
		ItemHandler.Instance.EditMode();
	}

	void Create(RaycastHit i_Hit)
	{
		if (m_TmpTask.CompareTag("Move") || m_TmpTask.CompareTag("GetOut"))
		{
			m_TmpTask.transform.position = new Vector3(m_TmpTask.transform.position.x, 0, m_TmpTask.transform.position.z);
			ReleaseOrder();
		}

		else if (m_TmpTask.CompareTag("PickUp")){
			if (i_Hit.transform.parent.CompareTag("Storage") || i_Hit.transform.parent.CompareTag("Station"))
			{
				m_TmpTask.GetComponent<Task>().SetInteractable(i_Hit.transform.parent.gameObject);
				m_TmpTask.transform.position = i_Hit.transform.position;
				ReleaseOrder();
			}
			else
				Cancel();

		}
		else if (m_TmpTask.CompareTag("Drop")){
			if (i_Hit.transform.parent.CompareTag("Storage")
			|| i_Hit.transform.CompareTag("Ground") 
			|| i_Hit.transform.parent.CompareTag("Station"))
			{
				if (!i_Hit.transform.CompareTag("Ground"))
				{
					m_TmpTask.GetComponent<Task>().SetInteractable(i_Hit.transform.parent.gameObject);
					m_TmpTask.transform.position = i_Hit.transform.position;
				}
				else
					m_TmpTask.transform.position = new Vector3(m_TmpTask.transform.position.x, 0, m_TmpTask.transform.position.z);
				ReleaseOrder();
			}
			//TODO if sol et ANYWHERE
			else
				Cancel();

		}
		else if (m_TmpTask.CompareTag("Use"))
		{
			if (i_Hit.transform.parent.CompareTag("Station"))
			{
				m_TmpTask.GetComponent<Task>().SetInteractable(i_Hit.transform.parent.gameObject);
				m_TmpTask.transform.position = i_Hit.transform.position;
				ReleaseOrder();
			}
			else
				Cancel();
		}
		else if (m_TmpTask.CompareTag("GetIn"))
		{
			if (i_Hit.transform.parent.CompareTag("Vehicule"))
			{
				m_TmpTask.GetComponent<Task>().SetInteractable(i_Hit.transform.parent.gameObject);
				m_TmpTask.transform.position = i_Hit.transform.position;
				ReleaseOrder();
			}
			else
				Cancel();
		}
	}
	void Cancel()
	{
		if (m_TmpTask)
		{
			Destroy(m_TmpTask);
			ReleaseOrder();
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
				Create(hit);
			else if (Input.GetMouseButton(1))
				Cancel();
		}
	}
}
