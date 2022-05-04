using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timeline : MonoBehaviour
{
	private GameObject m_Workers;
	public bool Running = false;
	public int Working = 0;
	public int Timer = 0;
	private float m_Timer = 0;
	[SerializeField]
	private Button m_PlayButton;
	[SerializeField]
	private Button m_StopButton;
	void Awake()
	{
		m_Workers = GameObject.Find("Worker");
	}
	void Start()
	{
	}

	public void Play()
	{
		foreach (Transform child in m_Workers.transform)
		{
			child.gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = true;
			child.gameObject.GetComponent<Worker>().Go();
		}
		Working = m_Workers.transform.childCount;
		Running = true;
	}

	public void Stop()
	{
		m_StopButton.gameObject.SetActive(false);
		m_PlayButton.gameObject.SetActive(true);
		foreach (Transform child in m_Workers.transform)
		{
			child.gameObject.GetComponent<Worker>().Reset();
			child.gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
		}
		Running = false;
		m_Timer = 0;
	}

	void FixedUpdate()
	{
		if (Running)
		{
			if (Working <= 0 && Timer < m_Timer)
				Stop();
			else
			{
				m_Timer += Time.deltaTime;
			}
		}
	}
}
