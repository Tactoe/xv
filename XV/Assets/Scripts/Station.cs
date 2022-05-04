using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Station : MonoBehaviour
{
	public GameObject myPrefab;
	// Start is called before the first frame update
	public void Start()
	{
	}

	// Update is called once per frame
	public void Update()
	{
		
	}

	public void DropIn(Transform i_parent)
	{
		//Get the object in the worker's hands and destroy it
		if (transform.Find("Slot").childCount > 0)
			return;
		GameObject myObj = i_parent.GetChild(0).gameObject;
        myObj.transform.parent = transform.Find("Slot");
        myObj.transform.localPosition = Vector3.zero;
		
	}

	public void PickUp(Transform i_pos)
	{
		if (transform.Find("Slot").childCount == 0)
			return;
		GameObject myObj = transform.Find("Slot").GetChild(0).gameObject;//objet sur la table
        myObj.transform.parent = i_pos;
		myObj.transform.localPosition = Vector3.zero;
	}

    public void Use()
	{
        //destruction object surla table
        Destroy(transform.Find("Slot").GetChild(0).gameObject);

        //instantiation du nouvel objet 
        GameObject tmp =  Instantiate(myPrefab, transform.Find("Slot"));
		tmp.transform.Find("Hitbox").gameObject.GetComponent<BoxCollider>().enabled = false;
		tmp.transform.Find("Hitbox").gameObject.GetComponent<UnityEngine.AI.NavMeshObstacle>().enabled = false;
        tmp.transform.localPosition = Vector3.zero;		
	}


}
