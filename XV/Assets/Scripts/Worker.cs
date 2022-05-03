using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Worker : MonoBehaviour
{
	public bool Loop;
	public Animator Animator;
	public Transform Tasks;
	public UnityEngine.AI.NavMeshAgent NavAgent;
	private Camera m_camera;
	private Vector3[] m_origin = new Vector3[3];
	private Timeline m_timeline;
	[SerializeField]
	private int m_taskIndex;
	[SerializeField]
	private bool m_over = false;
	private bool m_moving = false;
	void Awake()
	{
		m_timeline = GameObject.Find("Timeline").GetComponent<Timeline>();
		m_camera = GameObject.Find("Main Camera").GetComponent<Camera>();
		Tasks = transform.GetChild(1);
	}
	void Start()
	{
		m_origin[0] = transform.localPosition;
		m_origin[1] = transform.localEulerAngles;
		m_origin[2] = transform.localScale;
		m_taskIndex = 0;
	}
	
	public void Move()
	{
		Tasks.parent = null;
		if (m_taskIndex < Tasks.childCount)
		{
			m_moving = true;
			NavAgent.SetDestination(Tasks.GetChild(m_taskIndex).position);
		}
		else 
		{
			m_taskIndex = 0;
			if (!m_over)
			{
				m_over = true;
				m_timeline.Working -= 1;
			}
			if (Loop)
			{
				m_moving = true;
				NavAgent.SetDestination(Tasks.GetChild(m_taskIndex).position);
			}
		}
	}
	public void Reset()
	{
		StopAllCoroutines();
		m_moving = false;
		m_over = false;
		m_taskIndex = 0;
		transform.localPosition = m_origin[0];
		transform.localEulerAngles = m_origin[1];
		transform.localScale = m_origin[2];		
		NavAgent.SetDestination(transform.position);
		Tasks.parent = transform;
		Tasks.parent.position = new Vector3(transform.position.x, 0, transform.position.z);
	}
	public IEnumerator Do()
	{
		yield return new WaitForSeconds(0.5f);
		m_taskIndex += 1;
		Move();
	}

	void Update()
	{
		if (!NavAgent.pathPending && m_moving)
		{
			if (NavAgent.remainingDistance <= NavAgent.stoppingDistance)
			{
				if (!NavAgent.hasPath || NavAgent.velocity.sqrMagnitude == 0f)
				{
					m_moving = false;
					StartCoroutine(Do());
				}
			}
		}
		if (NavAgent.remainingDistance < 0.1f && m_moving)
		{
		}
		Animator.SetFloat("Speed", NavAgent.velocity.magnitude);
	}
}
