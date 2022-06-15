using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timeline : MonoBehaviour
{
	private GameObject m_Clone;
	private GameObject m_Scene;
	public bool Running = false;
	public int Working = 0;
	public int Timer = 0;
	[SerializeField]
	private float m_Timer = 0;
	[SerializeField]
	private TextMeshProUGUI m_WorkerCount;
	[SerializeField]
	private TMP_InputField m_InputTimer;
	[SerializeField]
	private TextMeshProUGUI m_RunTimer;
	[SerializeField]
	private Button m_PlayButton;
	[SerializeField]
	private Button m_StopButton;
	
	[SerializeField]
	private Toggle m_Toggle;
	private GameObject m_Camera;
	[SerializeField]
	private TextMeshProUGUI m_LogText;
	private string m_Log = "";
	

	void Awake()
	{
		m_Camera = GameObject.Find("Main Camera");
	}

	void Clone()
	{
		m_Scene = GameObject.Find("Scene");
		m_Clone = Instantiate(m_Scene);
		Item[] items = m_Clone.GetComponentsInChildren<Item>();
		foreach (Item item in items)
		{
			GameObject itemGO = item.transform.parent.gameObject;
            if (item.Data.ColorOverride.Count > 0)
            {
                ColorOverrider colorOverrider = itemGO.AddComponent<ColorOverrider>();
                colorOverrider.ApplyColorOverride(item.Data.ColorOverride);
                Destroy(colorOverrider);
            }
		}
	}

	public void Play()
	{
		Clone();
		m_Log = "";
		Timer = (m_InputTimer.text == "") ? 0 : int.Parse(m_InputTimer.text);
		m_InputTimer.gameObject.SetActive(false);
		m_RunTimer.gameObject.SetActive(true);
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
		Destroy(m_Clone);
		m_InputTimer.gameObject.SetActive(true);
		m_RunTimer.gameObject.SetActive(false);
		m_StopButton.gameObject.SetActive(false);
		m_PlayButton.gameObject.SetActive(true);
		Running = false;
		m_Timer = 0;
		m_Scene.SetActive(true);
		m_Camera.GetComponent<FFmpegOut.CameraCapture>().RecorderOff();
	}

	void FixedUpdate()
	{
		if (m_LogText.text != m_Log)
			m_LogText.text = m_Log;
		if (Running)
		{
			m_WorkerCount.text = Working.ToString();
			if (Working <= 0 && Timer < m_Timer)
				Stop();
				m_Timer += Time.deltaTime;
			m_RunTimer.text = (m_Timer > Timer) ? "0" : (Timer - (int)m_Timer).ToString();
		}
		else
		{
			Working = 0;
			if (m_Scene)
			{
				foreach (Transform child in m_Scene.transform)
				{
					if (child.gameObject.CompareTag("Worker"))
					{
						Working++;
					}
				}
				m_WorkerCount.text = Working.ToString();
			}
			else 
				m_Scene = GameObject.Find("Scene");
		}
	}

	public void AddLogEntry(string i_Entry)
	{
		m_Log += m_Timer.ToString("0.00") + "-" + i_Entry + '\n';
	}

	public void Pause()
	{
		Time.timeScale = 0f;
	}
	public void Normal()
	{
		Time.timeScale = 1f;
	}

	public void Fast()
	{
		Time.timeScale = 2f;
	}
}