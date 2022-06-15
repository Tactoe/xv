using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TaskCreator : MonoBehaviour
{
	[SerializeField]
	private GameObject m_TaskPrefab;
	private Camera m_Camera;
	static private GameObject m_TmpTask;
	[SerializeField]
	static private Timeline m_Timeline;
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
			if (i_Hit.transform.parent.gameObject.GetComponent<Storage>() != null || i_Hit.transform.parent.gameObject.GetComponent<Station>() != null || i_Hit.transform.parent.gameObject.GetComponent<Ressource>() != null)
			{
				m_TmpTask.GetComponent<Task>().SetInteractable(i_Hit.transform.parent.gameObject);
				m_TmpTask.transform.position = i_Hit.transform.position;
				ReleaseOrder();
			}
			else
				Cancel();

		}
		else if (m_TmpTask.CompareTag("Drop")){
			if (i_Hit.transform.parent.gameObject.GetComponent<Storage>() != null
			|| i_Hit.transform.CompareTag("Ground") 
			|| i_Hit.transform.parent.gameObject.GetComponent<Station>() != null)
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
			if (i_Hit.transform.parent.gameObject.GetComponent<Station>() != null)
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
			if (i_Hit.transform.parent.gameObject.GetComponent<Vehicle>() != null)
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

	void Update()
	{
		if (m_TmpTask)
		{
        	bool mouseInScene = !EventSystem.current.IsPointerOverGameObject();
			if (m_Timeline.Running)
			{
				Cancel();
				return ;
			}
			RaycastHit hit;
			Ray ray = m_Camera.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit))
				m_TmpTask.transform.position = hit.point;
			if (Input.GetMouseButton(0) && mouseInScene)
				Create(hit);
			else if (Input.GetMouseButton(1))
				Cancel();
		}
	}
}
