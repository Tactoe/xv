using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage : MonoBehaviour
{
	public int Busy;
	public GameObject myPrefab;
	
	public void Start()
	{
		Busy = (transform.Find("Target")) ? 0 : -1;
	}

	public void ToggleBusy()
	{
		if (Busy == 0)
			Busy = 1;
	}

	public void Update()
	{
		
	}

	public void DropIn(Transform i_parent)
	{
		//Get the object in the worker's hands and destroy it
		GameObject myObj = i_parent.GetChild(0).gameObject;
		Destroy(myObj);
		Busy = 0;
	}

	public void PickUp(Transform i_pos)
	{
		GameObject tmp =  Instantiate(myPrefab, i_pos);
		tmp.transform.Find("Hitbox").gameObject.GetComponent<BoxCollider>().enabled = false;
		tmp.transform.Find("Hitbox").gameObject.GetComponent<UnityEngine.AI.NavMeshObstacle>().enabled = false;
		tmp.transform.Find("Hitbox").Find("InteractionHitbox").gameObject.GetComponent<BoxCollider>().enabled = false;

		tmp.transform.localPosition = Vector3.zero;
		Busy = 0;
	}

}
