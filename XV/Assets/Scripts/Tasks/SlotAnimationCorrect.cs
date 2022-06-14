using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotAnimationCorrect : MonoBehaviour
{
	[SerializeField] private Transform m_AnimSlot;

	void FixedUpdate()
	{
		transform.position = m_AnimSlot.position;
	}
}
