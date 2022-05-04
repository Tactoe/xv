using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Vehicule : MonoBehaviour
{
	public GameObject Driver;
	[SerializeField]
	private Transform m_Dismount;
	[SerializeField]
	private Transform m_Seat;
	[SerializeField]
	private NavMeshAgent m_NavAgent;

    void Awake()
    {
        m_NavAgent = gameObject.GetComponent<NavMeshAgent>();
        m_Seat = transform.GetChild(3);
        m_Dismount = transform.GetChild(4);
    }

	public void GetOut()
	{
		Driver = null;
		Driver.GetComponent<NavMeshAgent>().enabled = true;
		Driver.transform.position = m_Dismount.position;
		m_NavAgent.enabled = true;
	}

	public void GetIn(GameObject i_Driver)
	{
		Driver = i_Driver;
		Driver.GetComponent<NavMeshAgent>().enabled = false;
		Driver.transform.position = m_Seat.position;
		m_NavAgent.enabled = true;
	}

    void Update()
    {
        if (Driver)
		{
			if (Driver.GetComponent<NavMeshAgent>().destination != m_NavAgent.destination)
				m_NavAgent.SetDestination(Driver.GetComponent<NavMeshAgent>().destination);
		}
    }
}
