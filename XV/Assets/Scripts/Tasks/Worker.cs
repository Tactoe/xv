using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Worker : MonoBehaviour
{
	public bool Loop;
	public Animator Animator;
	public Transform Tasks;
	public NavMeshAgent NavAgent;
	[HideInInspector]
	public Vector3 Destination;
	[HideInInspector]
	public bool Moving = false;
	[HideInInspector]
	public GameObject Vehicle;
	public int TaskIndex;

	private Transform m_Slot;
	private Vector3[] m_Origin = new Vector3[3];
	private bool m_CheckStuck = false;
	private Camera m_Camera;
	private Timeline m_Timeline;
	[SerializeField]
	private bool m_Over = false;

	// This script handle the logic of Workers
	// It goes down the worker's assigned list of tasks, completing them one after the other, looping if needed
	// It also handles the logic of all tasks, and objects interacted with
	// A worker should only carry a single light ressource

	void Awake()
	{
		m_Timeline = GameObject.Find("Timeline").GetComponent<Timeline>();
		m_Camera = GameObject.Find("Main Camera").GetComponent<Camera>();
		Tasks = transform.Find("Tasks");
		m_Slot = gameObject.transform.Find("Slot");
	}

	public void Go()
	{
		m_Origin[0] = transform.localPosition;
		m_Origin[1] = transform.localEulerAngles;
		m_Origin[2] = transform.localScale;
		Tasks.parent = Tasks.parent.parent;
		TaskIndex = -1;
		NextDoableTask();
		if (Tasks.transform.childCount > 0)
		{
			Move();
		}
		else
		{
			m_Over = true;
			m_Timeline.Working -= 1;
		}
	}

	void SetCourse()
	{
		if (Tasks.GetChild(TaskIndex).GetComponent<Task>().Target)
			Destination = Tasks.GetChild(TaskIndex).GetComponent<Task>().Interactable.transform.Find("Target").position;
		else
			Destination = Tasks.GetChild(TaskIndex).position;
		Moving = true;
		if (NavAgent.enabled)
		NavAgent.SetDestination(Destination);
	}
	void Move()
	{
		if (TaskIndex < Tasks.childCount)
			SetCourse();
		else 
		{
			TaskIndex = -1;
			NextDoableTask();
			if (!m_Over)
			{
				m_Over = true;
				m_Timeline.Working -= 1;
			}
			if (Loop)
				SetCourse();
			else if (!Vehicle)
				NavAgent.isStopped = true;

		}
	}

	public IEnumerator Do()
	{
		if (!Vehicle)
			NavAgent.isStopped = true;
		Moving = false;
		string tag = Tasks.GetChild(TaskIndex).tag;

		GameObject my_interact = Tasks.GetChild(TaskIndex).gameObject.GetComponent<Task>().Interactable;
		switch(tag) 
		{
			case "GetIn":
					my_interact.GetComponent<Vehicle>().GetIn(gameObject);
				break;
			case "GetOut":
				if (Vehicle)
					Vehicle.GetComponent<Vehicle>().GetOut();
				break;
			case "PickUp":
				if(Vehicle){
					Vehicle.GetComponent<Vehicle>().PickUp(my_interact);
				}
				else{
					if (my_interact.GetComponent<Ressource>() != null && m_Slot.childCount == 0)
					{
						Animator.SetBool("Carrying", true);
						yield return new WaitForSeconds(0.75f);
						PickUpGround(my_interact, m_Slot);
					}
					if (my_interact.GetComponent<Storage>() != null && m_Slot.childCount == 0)
					{
						my_interact.GetComponent<Storage>().ToggleBusy();
						Animator.SetBool("Carrying", true);
						yield return new WaitForSeconds(0.75f);
						my_interact.GetComponent<Storage>().PickUp(m_Slot);
					}
					else if (my_interact.GetComponent<Station>() != null && m_Slot.childCount == 0 && my_interact.transform.Find("Slot").childCount > 0)
					{
						my_interact.GetComponent<Station>().ToggleBusy();
						Animator.SetBool("Carrying", true);
						yield return new WaitForSeconds(0.75f);
						my_interact.GetComponent<Station>().PickUp(m_Slot);
					}
				}
				break;
			case "Drop":
				if (!my_interact)
				{
					Animator.SetBool("Carrying", false);
					yield return new WaitForSeconds(0.75f);					
					DropGround(m_Slot);
				}
				else if (my_interact.GetComponent<Storage>() != null && m_Slot.childCount > 0)
				{
					my_interact.GetComponent<Storage>().ToggleBusy();
					Animator.SetBool("Carrying", false);
					yield return new WaitForSeconds(0.75f);
					my_interact.GetComponent<Storage>().DropIn(m_Slot);
				}
				else if (my_interact.GetComponent<Station>() != null && m_Slot.childCount > 0 && my_interact.transform.Find("Slot").childCount ==0)
				{
					my_interact.GetComponent<Station>().ToggleBusy();
					Animator.SetBool("Carrying", false);
					yield return new WaitForSeconds(0.75f);
					my_interact.GetComponent<Station>().DropIn(m_Slot);
				}
				break;
			case "Use":
				if (my_interact.transform.Find("Slot").childCount > 0)
				{
					Animator.SetBool("Interacting", true);
					my_interact.GetComponent<Station>().ToggleBusy();
					yield return new WaitForSeconds(5f);
					my_interact.GetComponent<Station>().Use();
					Animator.SetBool("Interacting", false);
				}
				break;
		}

		float wait = Tasks.GetChild(TaskIndex).gameObject.GetComponent<Task>().Wait;
		yield return new WaitForSeconds(wait);

		if (Tasks.GetChild(TaskIndex).gameObject.GetComponent<Task>().Data.TaskDescription != "")
			m_Timeline.AddLogEntry(gameObject.GetComponentInChildren<Item>().Data.ItemName + " : " + Tasks.GetChild(TaskIndex).gameObject.GetComponent<Task>().Data.TaskDescription);
		else
			m_Timeline.AddLogEntry(gameObject.GetComponentInChildren<Item>().Data.ItemName + " : " + Tasks.GetChild(TaskIndex).gameObject.GetComponent<Task>().Data.PrefabName);
		NextDoableTask();
		if(!Vehicle)
			NavAgent.isStopped = false;
		Move();
	}

	void NextDoableTask()
	{
		while(TaskIndex < Tasks.childCount)
		{
			TaskIndex += 1;
			if (TaskIndex >= Tasks.childCount)
				return;
			string tag = Tasks.GetChild(TaskIndex).tag;
			if ((tag == "GetIn" && Vehicle) ||
				(tag == "GetOut" && !Vehicle) ||
				(tag == "Use" && Vehicle) ||
				(tag == "PickUp" && Vehicle && Vehicle.GetComponent<Vehicle>().Carrying) ||
				(tag == "Drop" && Vehicle && !Vehicle.GetComponent<Vehicle>().Carrying) ||
				(tag == "PickUp" && !Vehicle && m_Slot.childCount > 0) ||
				(tag == "Drop" && !Vehicle && m_Slot.childCount == 0))
				continue;
			return ;
		}
	}

	void PickUpGround(GameObject my_interact, Transform m_Slot)
	{
		my_interact.transform.parent = m_Slot;
		my_interact.transform.position = m_Slot.position;
		my_interact.transform.Find("Hitbox").gameObject.GetComponent<BoxCollider>().enabled = false;
		my_interact.transform.Find("Hitbox").gameObject.GetComponent<UnityEngine.AI.NavMeshObstacle>().enabled = false;
	}

	void DropGround(Transform m_Slot)
	{
		if (m_Slot.childCount > 0)
		{
			Transform dropped = m_Slot.GetChild(0);
			dropped.parent = transform.parent;
			dropped.Find("Hitbox").gameObject.GetComponent<BoxCollider>().enabled = true;
			dropped.Find("Hitbox").gameObject.GetComponent<UnityEngine.AI.NavMeshObstacle>().enabled = true;
			dropped.localPosition = new Vector3 (dropped.localPosition.x, 0.045f, dropped.localPosition.z);
		}
		else if (Vehicle)
		{
			Vehicle.GetComponent<Vehicle>().Drop();
		}
	}

	private IEnumerator IsStuck(Vector3 i_Pos)
	{
		m_CheckStuck = true;
		yield return new WaitForSeconds(0.75f);
		if (m_CheckStuck && Moving && Vector3.Distance(i_Pos, transform.position) < 0.01)
		{
			StopAllCoroutines();
			TaskIndex += 1;
			Move();
		}
		m_CheckStuck = false;
	}

	void Update()
	{
		if (Moving)
		{
			if (!NavAgent.pathPending && NavAgent.enabled)
			{
				if (!m_CheckStuck && !NavAgent.isStopped)
					StartCoroutine(IsStuck(transform.position));
				if (NavAgent.remainingDistance <= NavAgent.stoppingDistance)
				{
					if (!NavAgent.hasPath || NavAgent.velocity.sqrMagnitude == 0f)
					{
						StartCoroutine(Do());
					}
				}
			}
			if (!Tasks.GetChild(TaskIndex).GetComponent<Task>().Target)
			{
				if (Destination != Tasks.GetChild(TaskIndex).position)
					SetCourse();
			}
			else 
			{
				if (Destination != Tasks.GetChild(TaskIndex).GetComponent<Task>().Interactable.transform.Find("Target").position)
					SetCourse();
			}
			if (Tasks.GetChild(TaskIndex).GetComponent<Task>().Target)
			{
				int busy = -1;
				if (Tasks.GetChild(TaskIndex).GetComponent<Task>().Interactable.tag == "Station"){
					busy = Tasks.GetChild(TaskIndex).GetComponent<Task>().Interactable.GetComponent<Station>().Busy;
					}
				else if (Tasks.GetChild(TaskIndex).GetComponent<Task>().Interactable.tag == "Storage"){
					busy = Tasks.GetChild(TaskIndex).GetComponent<Task>().Interactable.GetComponent<Storage>().Busy;
					}
				if (busy > 0)
				{
					print("WAIT");
					m_CheckStuck = false;
					if(!Vehicle)
						NavAgent.isStopped = true;
				}
				else if (busy == 0)
				{
					print("GO");
					if(!Vehicle)
						NavAgent.isStopped = false;
				}
				busy = -1;
			}
		}
		Animator.SetFloat("Speed", NavAgent.velocity.magnitude);
	}

	void OnTriggerStay(Collider other)
		{
		GameObject m_inter = other.transform.parent.parent.gameObject;
		if(Moving
		&& m_inter == Tasks.GetChild(TaskIndex).gameObject.GetComponent<Task>().Interactable
		&&!Tasks.GetChild(TaskIndex).gameObject.GetComponent<Task>().Target)
		{
			StartCoroutine(Do());
		}
		}
}
