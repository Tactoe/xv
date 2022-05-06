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
		Tasks.parent = Tasks.parent.parent.parent;
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
		}
	}

	public IEnumerator Do()
	{
		Moving = false;
		string tag = Tasks.GetChild(TaskIndex).tag;
		// if (tag == "GetIn")
		// {
		// }
		// else if (tag == "GetOut")
		// {
		// }
		// else if( tag == "PickUp")
		// {
		// }
		// else if(tag == )

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
					Animator.SetBool("Carrying", true);
					yield return new WaitForSeconds(1f);
					if (my_interact.GetComponent<Storage>() != null && slot.childCount == 0){
						my_interact.GetComponent<Storage>().PickUp(slot);
					}
					else if(my_interact.GetComponent<Station>() != null && slot.childCount == 0){
						my_interact.GetComponent<Station>().PickUp(slot);
					}
				}
				break;
			case "Drop":
				Animator.SetBool("Carrying", false);
				yield return new WaitForSeconds(1f);
				if (!my_interact)
					DropGround(slot);
				else if (my_interact.GetComponent<Storage>() != null && slot.childCount > 0){
					my_interact.GetComponent<Storage>().DropIn(slot);
				}
				else if(my_interact.GetComponent<Station>() != null && slot.childCount > 0){
					my_interact.GetComponent<Station>().DropIn(slot);
				}
				break;
			case "Use":
				if(my_interact.GetComponent<Station>() != null){
					my_interact.GetComponent<Station>().Use();
				}
				break;
		}

		float wait = Tasks.GetChild(TaskIndex).gameObject.GetComponent<Task>().Wait;
		yield return new WaitForSeconds(wait);
		TaskIndex += 1;
		Move();
	}

	void DropGround(Transform slot)
	{
		if (slot.childCount > 0)
		{
			slot.GetChild(0).parent = slot.parent.parent.parent;
		}
		else if (Vehicule)
		{
			Vehicule.GetComponent<Vehicule>().Drop();
		}
	}

	private IEnumerator IsStuck(Vector3 i_Pos)
	{
		Debug.Log("Is...");
		yield return new WaitForSeconds(3f);
		Debug.Log("It.." + Vector3.Distance(i_Pos, transform.position));
		if (Vector3.Distance(i_Pos, transform.position) < 0.05)
		{
		Debug.Log("STUCK");
			StopAllCoroutines();
			TaskIndex += 1;
			Move();
		}
	}

	void Update()
	{
		if (!NavAgent.pathPending && Moving && NavAgent.enabled)
		{
			StartCoroutine(IsStuck(transform.position));
			if (NavAgent.remainingDistance <= NavAgent.stoppingDistance)
			{
				if (!NavAgent.hasPath || NavAgent.velocity.sqrMagnitude == 0f)
				{
					StartCoroutine(Do());
				}
			}
		}
		if (Moving)
		{
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
