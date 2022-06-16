using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotAnimationCorrect : MonoBehaviour
{
	[SerializeField] private Transform m_AnimSlot;

	//This script moves a carried ressource into the worker's hand as he's moving around.

	void FixedUpdate()
	{
		transform.position = m_AnimSlot.position;
	}
}
