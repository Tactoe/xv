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

	public void SetInteractable(GameObject i_Interactable)
	{
		Interactable = i_Interactable;
		Data.HasParent = true;
		Data.RelatedParentID = Interactable.transform.GetSiblingIndex();
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
		transform.LookAt(m_Camera.transform);
		transform.localScale = Vector3.one * (Vector3.Distance(transform.position, m_Camera.transform.position) / 7f);
		if (Data.HasParent && !Interactable)
			Destroy(gameObject);
	}
}
