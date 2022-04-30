using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Worker : MonoBehaviour
{
	public Animator Animator;
	public UnityEngine.AI.NavMeshAgent NavAgent;
	private Camera m_camera;

	void Awake()
	{
		m_camera = GameObject.Find("Main Camera").GetComponent<Camera>();
	}
	void Start()
	{
		
	}

	void Update()
	{
		if (Input.GetMouseButton(0))
			{
				RaycastHit hit;
				Ray ray = m_camera.ScreenPointToRay(Input.mousePosition);
				if (Physics.Raycast(ray, out hit))
				{
					NavAgent.SetDestination(hit.point);
				}
			}
		Animator.SetFloat("Speed", NavAgent.velocity.magnitude);
	}
}
