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
	private Camera m_Camera;
	private Vector3[] m_Origin = new Vector3[3];
	private Timeline m_Timeline;
	[SerializeField]
	private int m_TaskIndex;
	[SerializeField]
	private bool m_Over = false;
	private bool m_Moving = false;

	void Awake()
	{
		m_Timeline = GameObject.Find("Timeline").GetComponent<Timeline>();
		m_Camera = GameObject.Find("Main Camera").GetComponent<Camera>();
		Tasks = transform.GetChild(2);
	}

	public void Go()
	{
		m_Origin[0] = transform.localPosition;
		m_Origin[1] = transform.localEulerAngles;
		m_Origin[2] = transform.localScale;
		m_TaskIndex = 0;
		Move();
	}

	public void Reset()
	{
		StopAllCoroutines();
		m_Moving = false;
		m_Over = false;
		m_TaskIndex = 0;
		transform.localPosition = m_Origin[0];
		transform.localEulerAngles = m_Origin[1];
		transform.localScale = m_Origin[2];		
		NavAgent.SetDestination(transform.position);
		Tasks.parent = transform;
		Tasks.parent.position = new Vector3(transform.position.x, 0, transform.position.z);
	}

	void Move()
	{
		Tasks.parent = null;
		if (m_TaskIndex < Tasks.childCount)
		{
			m_Moving = true;
			NavAgent.SetDestination(Tasks.GetChild(m_TaskIndex).position);
		}
		else 
		{
			m_TaskIndex = 0;
			if (!m_Over)
			{
				m_Over = true;
				m_Timeline.Working -= 1;
			}
			if (Loop)
			{
				m_Moving = true;
				NavAgent.SetDestination(Tasks.GetChild(m_TaskIndex).position);
			}
		}
	}

	public IEnumerator Do()
	{
		m_Moving = false;
		string tag = Tasks.GetChild(m_TaskIndex).tag;
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

		Transform mPosSlot = gameObject.transform.Find("Slot");
		
		switch(tag) 
		{
			case "GetIn":
				Debug.Log("GetIn");
				break;
			case "GetOut":
				Debug.Log("GetOut");
				break;
			case "PickUp":
				Animator.SetBool("Carrying", true);
				yield return new WaitForSeconds(1f);
				Tasks.GetChild(m_TaskIndex).gameObject.GetComponent<Task>().Interactable.GetComponent<Storage>().PickUp(mPosSlot);
				break;
			case "Drop":
				Animator.SetBool("Carrying", false);
				yield return new WaitForSeconds(1f);
				Tasks.GetChild(m_TaskIndex).gameObject.GetComponent<Task>().Interactable.GetComponent<Storage>().DropIn(mPosSlot);
				break;
			case "Use":
				Debug.Log("Use");
				break;
		}

		yield return new WaitForSeconds(0.5f);
		m_TaskIndex += 1;
		Move();
	}

	void Update()
	{
		if (!NavAgent.pathPending && m_Moving && NavAgent.enabled)
		{
			if (NavAgent.remainingDistance <= NavAgent.stoppingDistance)
			{
				if (!NavAgent.hasPath || NavAgent.velocity.sqrMagnitude == 0f)
				{
					// m_Moving = false;
					StartCoroutine(Do());
				}
			}
		}
		Animator.SetFloat("Speed", NavAgent.velocity.magnitude);
	}

	void OnTriggerEnter(Collider other)
    {
		Debug.Log("lolilol");
		Debug.Log(other);
		Debug.Log(other.gameObject.name);
		Debug.Log(other.transform.parent.parent.gameObject.name);
		GameObject m_inter = other.transform.parent.parent.gameObject;
        if(m_Moving && m_inter == Tasks.GetChild(m_TaskIndex).gameObject.GetComponent<Task>().Interactable )
		{
			
			StartCoroutine(Do());
		}
    }
}
