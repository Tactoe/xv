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
	public GameObject Vehicule;
	public int TaskIndex;

	private Vector3[] m_Origin = new Vector3[3];
	private bool m_CheckStuck = false;
	private Camera m_Camera;
	private Timeline m_Timeline;
	[SerializeField]
	private bool m_Over = false;

	void Awake()
	{
		m_Timeline = GameObject.Find("Timeline").GetComponent<Timeline>();
		m_Camera = GameObject.Find("Main Camera").GetComponent<Camera>();
		Tasks = transform.Find("Tasks");
	}

	public void Go()
	{
		m_Origin[0] = transform.localPosition;
		m_Origin[1] = transform.localEulerAngles;
		m_Origin[2] = transform.localScale;
		Tasks.parent = Tasks.parent.parent;
		TaskIndex = 0;
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
			TaskIndex = 0;
			if (!m_Over)
			{
				m_Over = true;
				m_Timeline.Working -= 1;
			}
			if (Loop)
				SetCourse();
			else
				NavAgent.isStopped = true;
		}
	}

	public IEnumerator Do()
	{
		if (!Vehicule)
			NavAgent.isStopped = true;
		Moving = false;
		string tag = Tasks.GetChild(TaskIndex).tag;

		Transform slot = gameObject.transform.Find("Slot");
		GameObject my_interact = Tasks.GetChild(TaskIndex).gameObject.GetComponent<Task>().Interactable;
		switch(tag) 
		{
			case "GetIn":
				if (slot.childCount == 0)
					my_interact.GetComponent<Vehicule>().GetIn(gameObject);
				break;
			case "GetOut":
				if (Vehicule)
					Vehicule.GetComponent<Vehicule>().GetOut();
				break;
			case "PickUp":
				if(Vehicule){
					Vehicule.GetComponent<Vehicule>().PickUp(my_interact);
				}
				else{
					if (my_interact.CompareTag("Ressource") && slot.childCount == 0)
					{
						Animator.SetBool("Carrying", true);
						yield return new WaitForSeconds(1f);
						PickUpGround(my_interact, slot);
					}
					if (my_interact.GetComponent<Storage>() != null && slot.childCount == 0)
					{
						my_interact.GetComponent<Storage>().ToggleBusy();
						Animator.SetBool("Carrying", true);
						yield return new WaitForSeconds(1f);
						my_interact.GetComponent<Storage>().PickUp(slot);
					}
					else if (my_interact.GetComponent<Station>() != null && slot.childCount == 0 && my_interact.transform.Find("Slot").childCount > 0)
					{
						my_interact.GetComponent<Station>().ToggleBusy();
						Animator.SetBool("Carrying", true);
						yield return new WaitForSeconds(1f);
						my_interact.GetComponent<Station>().PickUp(slot);
					}
				}
				break;
			case "Drop":
				if (!my_interact)
					DropGround(slot);
				else if (my_interact.GetComponent<Storage>() != null && slot.childCount > 0)
				{
					my_interact.GetComponent<Storage>().ToggleBusy();
					Animator.SetBool("Carrying", false);
					yield return new WaitForSeconds(1f);
					my_interact.GetComponent<Storage>().DropIn(slot);
				}
				else if (my_interact.GetComponent<Station>() != null && slot.childCount > 0 && my_interact.transform.Find("Slot").childCount ==0)
				{
					my_interact.GetComponent<Station>().ToggleBusy();
					Animator.SetBool("Carrying", false);
					yield return new WaitForSeconds(1f);
					my_interact.GetComponent<Station>().DropIn(slot);
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
		TaskIndex += 1;
		if(!Vehicule)
			NavAgent.isStopped = false;
		Move();
	}

	void PickUpGround(GameObject my_interact, Transform slot)
	{
		my_interact.transform.parent = slot;
		my_interact.transform.position = slot.position;
		my_interact.GetComponent<BoxCollider>().enabled = false;
		my_interact.GetComponent<UnityEngine.AI.NavMeshObstacle>().enabled = false;
		my_interact.transform.Find("InteractionHitbox").gameObject.GetComponent<BoxCollider>().enabled = false;
	}

	void DropGround(Transform slot)
	{
		if (slot.childCount > 0)
		{
			slot.GetChild(0).parent = transform.parent;
		}
		else if (Vehicule)
		{
			Vehicule.GetComponent<Vehicule>().Drop();
		}
	}

	private IEnumerator IsStuck(Vector3 i_Pos)
	{
		m_CheckStuck = true;
		// Debug.Log("Is...");
		yield return new WaitForSeconds(1f);
		// Debug.Log("It.." + Vector3.Distance(i_Pos, transform.position));
		if (m_CheckStuck && Moving && Vector3.Distance(i_Pos, transform.position) < 0.01)
		{
		Debug.Log("STUCK");
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
					print (Tasks.GetChild(TaskIndex).GetComponent<Task>().Interactable.name + " is isk");
				if (busy > 0)
				{
					print("WAIT");
					m_CheckStuck = false;
					if(!Vehicule)
						NavAgent.isStopped = true;
				}
				else if (busy == 0)
				{
					print("GO");
					if(!Vehicule)
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
