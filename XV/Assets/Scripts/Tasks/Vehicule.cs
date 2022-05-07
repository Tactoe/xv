using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Vehicule : MonoBehaviour
{
	public GameObject Driver;
	private Transform m_Dismount;
	private Transform m_Seat;
	private NavMeshAgent m_NavAgent;
	private NavMeshObstacle m_NavObstacle;
	private Worker m_Worker;

    void Awake()
    {
        m_NavObstacle = gameObject.GetComponent<NavMeshObstacle>();
        m_NavAgent = gameObject.GetComponent<NavMeshAgent>();
        m_Seat = transform.Find("Seat");
        m_Dismount = transform.Find("Target");
		m_NavAgent.enabled = false;
    }

	public void GetOut()
	{
		m_NavAgent.enabled = false;
		transform.Find("Hitbox").gameObject.GetComponent<NavMeshObstacle>().enabled = true;
		Driver.GetComponent<NavMeshAgent>().enabled = true;
		Driver.transform.position = m_Dismount.position;
		m_Worker.Vehicule = null;
		Driver = null;
	}

	public void GetIn(GameObject i_Worker)
	{
		if (Driver)
			return;
		m_NavAgent.enabled = true;
		Driver = i_Worker;
		m_Worker = Driver.GetComponent<Worker>();
		transform.Find("Hitbox").gameObject.GetComponent<NavMeshObstacle>().enabled = false;
		Driver.GetComponent<NavMeshAgent>().enabled = false;
		m_Worker.Vehicule = gameObject;
	}

	public void PickUp(GameObject i_interact)
	{
		if (i_interact.GetComponentInChildren<Item>().Data.Tags.Contains(ItemTags.Heavy))
		{
			i_interact.transform.Find("Hitbox").gameObject.GetComponent<BoxCollider>().enabled = false;
			i_interact.transform.Find("Hitbox").gameObject.GetComponent<UnityEngine.AI.NavMeshObstacle>().enabled = false;
			i_interact.transform.parent = transform.Find("Slot");
			i_interact.transform.localPosition = Vector3.zero;
		}
	}
	public void Drop()
	{
		if (transform.Find("Slot").childCount == 1)
		{
			Transform item = transform.Find("Slot").GetChild(0);
			Debug.Log(item.gameObject.name);
			item.Find("Hitbox").gameObject.GetComponent<BoxCollider>().enabled = true;
			item.Find("Hitbox").gameObject.GetComponent<UnityEngine.AI.NavMeshObstacle>().enabled = true;
			item.parent = transform.parent;
		}
	}
	

    void Update()
    {
        if (Driver)
		{
			Driver.transform.position = m_Seat.position;
			Driver.transform.eulerAngles = m_Seat.eulerAngles;
			if (m_Worker.Destination != m_NavAgent.destination)
				m_NavAgent.SetDestination(m_Worker.Destination);
			if (!m_NavAgent.pathPending && m_Worker.Moving && m_NavAgent.enabled)
			{
				if (m_NavAgent.remainingDistance <= m_NavAgent.stoppingDistance)
				{
					if (!m_NavAgent.hasPath || m_NavAgent.velocity.sqrMagnitude == 0f)
					{
						StartCoroutine(m_Worker.Do());
					}
				}
			}
		}
    }

	void OnTriggerStay(Collider other)
    {
		if (Driver)
		{
			GameObject m_inter = other.transform.parent.parent.gameObject;
        	if (m_Worker.Moving && m_inter == m_Worker.Tasks.GetChild(m_Worker.TaskIndex).gameObject.GetComponent<Task>().Interactable)
			{
				StartCoroutine(m_Worker.Do());
			}	
		}
    }
}
