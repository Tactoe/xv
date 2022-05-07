using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timeline : MonoBehaviour
{
	private GameObject m_Clone;
	private GameObject m_Scene;
	public bool Running = false;
	public int Working = 0;
	public int Timer = 0;
	private float m_Timer = 0;
	[SerializeField]
	private Button m_PlayButton;
	[SerializeField]
	private Button m_StopButton;
	
	[SerializeField]
	private Toggle m_Toggle;
	[SerializeField]
	private GameObject m_Camera;

	void Clone()
	{
		m_Clone = Instantiate(m_Scene);
	}

	public void Play()
	{
		m_Scene = GameObject.Find("Scene");
		Clone();
		m_StopButton.gameObject.SetActive(true);
		m_PlayButton.gameObject.SetActive(false);
		Working = 0;
		foreach (Transform child in m_Clone.transform)
		{
			if (child.gameObject.CompareTag("Worker"))
			{
				child.gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = true;
				child.gameObject.GetComponent<Worker>().Go();
				Working++;
			}
		}
		Running = true;
		m_Scene.SetActive(false);

		if (m_Toggle.isOn){
			m_Camera.GetComponent<FFmpegOut.CameraCapture>().startStopRecord();
		}
	}

	public void Stop()
	{
		Debug.Log("arret");
		Destroy(m_Clone);
		m_StopButton.gameObject.SetActive(false);
		m_PlayButton.gameObject.SetActive(true);
		Running = false;
		m_Timer = 0;
		m_Scene.SetActive(true);
		m_Camera.GetComponent<FFmpegOut.CameraCapture>().RecorderOff();
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
