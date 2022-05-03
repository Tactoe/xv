using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timeline : MonoBehaviour
{
	private GameObject m_workers;
	public bool Running = false;
	public int Working = 0;
	public int Timer = 0;
	[SerializeField]
	private float m_timer = 0;
	[SerializeField]
	private Button m_playButton;
	[SerializeField]
	private Button m_stopButton;
	void Awake()
	{
		m_workers = GameObject.Find("Workers");
	}
	void Start()
	{
	}

	public void Play()
	{
		foreach (Transform child in m_workers.transform)
		{
			child.gameObject.GetComponent<Worker>().Move();
		}
		Working = m_workers.transform.childCount;
		Running = true;
	}

	public void Stop()
	{
		m_stopButton.gameObject.SetActive(false);
		m_playButton.gameObject.SetActive(true);
		foreach (Transform child in m_workers.transform)
		{
			child.gameObject.GetComponent<Worker>().Reset();
		}
		Running = false;
		m_timer = 0;
	}

	void FixedUpdate()
	{
		if (Running)
		{
			if (Working <= 0 && Timer < m_timer)
				Stop();
			else
			{
				m_timer += Time.deltaTime;
			}
		}
	}
}
