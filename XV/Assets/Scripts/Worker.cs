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
		string tag = Tasks.GetChild(m_TaskIndex).tag;
		if (tag == "GetIn")
		{
		}
		else if (tag == "GetOut")
		{
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
					m_Moving = false;
					StartCoroutine(Do());
				}
			}
		}
		Animator.SetFloat("Speed", NavAgent.velocity.magnitude);
	}
}
