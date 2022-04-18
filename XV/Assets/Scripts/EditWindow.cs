using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditWindow : MonoBehaviour
{
    public GameObject Target;

    public void ChangeTargetPosition(Vector3 i_newPos)
    {
        Target.transform.position = i_newPos;
    }
    
    public void ChangeTargetRotation(Vector3 i_newRot)
    {
        Target.transform.rotation =  Quaternion.Euler(i_newRot);
    }
    public void ChangeTargetScale(Vector3 i_newScale)
    {
        Target.transform.localScale = i_newScale;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
