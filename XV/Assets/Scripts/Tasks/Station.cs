using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Station : MonoBehaviour
{
	public int Busy;
	public GameObject myPrefab;
	public void Start()
	{
		Busy = (transform.Find("Target")) ? 0 : -1;
	}

	// This script handles the logic of object tagged as stations
	// These functions are called into Worker.cs when they are needed
	// Station can carry a single light ressource into their slot, and they "transform" them into myPrefab

	public void ToggleBusy()
	{
		if (Busy == 0)
			Busy = 1;
	}

	public void DropIn(Transform i_parent)
	{
		//Get the object in the worker's hands and destroy it
		GameObject myObj = i_parent.GetChild(0).gameObject;
        myObj.transform.parent = transform.Find("Slot");
        myObj.transform.localPosition = Vector3.zero;
		Busy = 0;
	}

	public void PickUp(Transform i_pos)
	{
		GameObject myObj = transform.Find("Slot").GetChild(0).gameObject;//objet sur la table
        myObj.transform.parent = i_pos;
		myObj.transform.localPosition = Vector3.zero;
		Busy = 0;
	}

    public void Use()
	{
        Destroy(transform.Find("Slot").GetChild(0).gameObject);

        GameObject tmp =  Instantiate(myPrefab, transform.Find("Slot"));
		tmp.transform.Find("Hitbox").gameObject.GetComponent<BoxCollider>().enabled = false;
		tmp.transform.Find("Hitbox").gameObject.GetComponent<UnityEngine.AI.NavMeshObstacle>().enabled = false;
        tmp.transform.localPosition = Vector3.zero;		
		Busy = 0;
	}


}
