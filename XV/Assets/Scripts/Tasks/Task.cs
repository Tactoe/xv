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
	public GameObject Interactable;
	public int Wait = 0;
	public bool Target;
	public TaskData Data;
	private bool m_Static = false;
	private Vector3 m_StaticPos;
	
	// This script handles the logic and visualization of tasks
	// It contains most of the task data, like what object is the task linked to or not, the position...

	void Awake()
	{
		m_Camera = GameObject.Find("Main Camera");
		m_Line = gameObject.GetComponent<LineRenderer>();
		m_Sprite = gameObject.GetComponent<SpriteRenderer>();
	}
	void Start()
	{
		Target = false;
		Data.RelatedWorkerID = transform.parent.parent.GetSiblingIndex();
		if (Interactable)
		{
			if (Interactable.transform.Find("Target"))
				Target = true;
		}
	}

	public void SetStatic ()
	{
		m_Static = true;
		m_StaticPos = transform.position;
	}

	public void SetInteractable(GameObject i_Interactable)
	{
		Interactable = i_Interactable;
		Data.HasParent = true;
		Data.RelatedParentID = Interactable.transform.GetSiblingIndex();
	}

	void CorrectScale(Transform i_Parent)
	{
		transform.LookAt(m_Camera.transform);
		transform.localScale = Vector3.one * (Vector3.Distance(transform.position, m_Camera.transform.position) / 7f);
	}

	void Update()
	{
		m_Line.SetPosition(0, transform.position);
		if (transform.GetSiblingIndex() > 0)
			m_Line.SetPosition(1, transform.parent.GetChild(transform.GetSiblingIndex() - 1).position);
		else
			m_Line.SetPosition(1, transform.parent.position);
		if (Interactable && Interactable.transform.position != transform.position)
			transform.position = Interactable.transform.position;
		else if (m_Static && m_StaticPos != transform.position)
			transform.position = m_StaticPos;
		CorrectScale(transform.parent);
		if (Data.HasParent && !Interactable && transform.parent.parent.name != "Scene(Clone)")
			Destroy(gameObject);
	}
}
