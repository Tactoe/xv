using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage : MonoBehaviour
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
		GameObject myObj = i_parent.GetChild(0).gameObject;
		Destroy(myObj);
	}

	public void PickUp(Transform i_pos)
	{
		GameObject tmp =  Instantiate(myPrefab, i_pos);
		tmp.transform.Find("Hitbox").gameObject.GetComponent<BoxCollider>().enabled = false;
		tmp.transform.Find("Hitbox").gameObject.GetComponent<UnityEngine.AI.NavMeshObstacle>().enabled = false;
		tmp.transform.Find("Hitbox").Find("InteractionHitbox").gameObject.GetComponent<BoxCollider>().enabled = false;

		tmp.transform.localPosition = Vector3.zero;
	}

}
