using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Task : MonoBehaviour
{
	private SpriteRenderer m_Sprite;
	private LineRenderer m_Line;
	private GameObject m_Camera;

	void Awake()
	{
		m_Camera = GameObject.Find("Main Camera");
		m_Line = gameObject.GetComponent<LineRenderer>();
		m_Sprite = gameObject.GetComponent<SpriteRenderer>();
	}
	void Start()
	{
		
	}
	void Update()
	{
		m_Line.SetPosition(0, transform.position);
		if (transform.GetSiblingIndex() > 0)
			m_Line.SetPosition(1, transform.parent.GetChild(transform.GetSiblingIndex() - 1).position);
		else
			m_Line.SetPosition(1, transform.parent.position);

		transform.LookAt(m_Camera.transform);
		transform.localScale = Vector3.one * (Vector3.Distance(transform.position, m_Camera.transform.position) / 7f);
	}
}
