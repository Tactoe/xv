using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Task : MonoBehaviour
{
	private SpriteRenderer m_sprite;
	private LineRenderer m_line;
	private GameObject m_camera;

	void Awake()
	{
		m_camera = GameObject.Find("Main Camera");
		m_line = gameObject.GetComponent<LineRenderer>();
		m_sprite = gameObject.GetComponent<SpriteRenderer>();
	}
	void Start()
	{
		
	}
	void Update()
	{
		m_line.SetPosition(0, transform.position);
		if (transform.GetSiblingIndex() > 0)
			m_line.SetPosition(1, transform.parent.GetChild(transform.GetSiblingIndex() - 1).position);
		else
			m_line.SetPosition(1, transform.parent.position);

		transform.LookAt(m_camera.transform);
		transform.localScale = Vector3.one * (Vector3.Distance(transform.position, m_camera.transform.position) / 7f);
	}
}
