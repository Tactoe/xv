using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TaskCreator : MonoBehaviour
{
	[SerializeField]
	private GameObject m_TaskPrefab;
	private Camera m_Camera;
	static public GameObject TmpTask;
	[SerializeField]
	static private Timeline m_Timeline;

	// This script handle the logic of creating and placing a task in the world
	// It is called from the 6 buttons in the UI element handling tasks

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
			if (TmpTask)
				Cancel();
			TmpTask =
			Instantiate(m_TaskPrefab, transform.parent.parent.parent.GetComponent<TaskList>().Worker.transform.Find("Tasks"));
			ItemHandler.Instance.PlaceOrderMode();
			TmpTask.name = m_TaskPrefab.name;
		}
	}

	void ReleaseOrder()
	{
		TmpTask = null;
		ItemHandler.Instance.EditMode();
	}

	void Create(RaycastHit i_Hit)
	{
		if (TmpTask.CompareTag("Move") || TmpTask.CompareTag("GetOut"))
		{
			TmpTask.transform.position = new Vector3(TmpTask.transform.position.x, 0, TmpTask.transform.position.z);
			TmpTask.GetComponent<Task>().SetStatic();
			ReleaseOrder();
		}

		else if (TmpTask.CompareTag("PickUp")){
			if (i_Hit.transform.parent.gameObject.GetComponent<Storage>() != null || i_Hit.transform.parent.gameObject.GetComponent<Station>() != null || i_Hit.transform.parent.gameObject.GetComponent<Ressource>() != null)
			{
				TmpTask.GetComponent<Task>().SetInteractable(i_Hit.transform.parent.gameObject);
				TmpTask.transform.position = i_Hit.transform.position;
				ReleaseOrder();
			}
			else
				Cancel();

		}
		else if (TmpTask.CompareTag("Drop")){
			if (i_Hit.transform.parent.gameObject.GetComponent<Storage>() != null
			|| i_Hit.transform.CompareTag("Ground") 
			|| i_Hit.transform.parent.gameObject.GetComponent<Station>() != null)
			{
				if (!i_Hit.transform.CompareTag("Ground"))
				{
					TmpTask.GetComponent<Task>().SetInteractable(i_Hit.transform.parent.gameObject);
					TmpTask.transform.position = i_Hit.transform.position;
				}
				else
					TmpTask.transform.position = new Vector3(TmpTask.transform.position.x, 0, TmpTask.transform.position.z);
				ReleaseOrder();
			}
			//TODO if sol et ANYWHERE
			else
				Cancel();

		}
		else if (TmpTask.CompareTag("Use"))
		{
			if (i_Hit.transform.parent.gameObject.GetComponent<Station>() != null)
			{
				TmpTask.GetComponent<Task>().SetInteractable(i_Hit.transform.parent.gameObject);
				TmpTask.transform.position = i_Hit.transform.position;
				ReleaseOrder();
			}
			else
				Cancel();
		}
		else if (TmpTask.CompareTag("GetIn"))
		{
			if (i_Hit.transform.parent.gameObject.GetComponent<Vehicle>() != null)
			{
				TmpTask.GetComponent<Task>().SetInteractable(i_Hit.transform.parent.gameObject);
				TmpTask.transform.position = i_Hit.transform.position;
				ReleaseOrder();
			}
			else
				Cancel();
		}
	}
	void Cancel()
	{
		if (TmpTask)
		{
			Destroy(TmpTask);
			ReleaseOrder();
		}
	}

	void Update()
	{
		if (TmpTask)
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
				TmpTask.transform.position = hit.point;
			if (Input.GetMouseButton(0) && mouseInScene)
				Create(hit);
			else if (Input.GetMouseButton(1))
				Cancel();
		}
	}
}
