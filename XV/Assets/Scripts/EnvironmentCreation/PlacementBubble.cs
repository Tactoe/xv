using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementBubble : MonoBehaviour
{
    public bool CanBePlaced = true;
    [SerializeField]
    Color m_CorrectColor;
    [SerializeField]
    Color m_WrongColor;
    List<int> m_NearbyObjects;

    MeshRenderer m_Mesh;    
    // Start is called before the first frame update
    void Start()
    {
        m_Mesh = GetComponent<MeshRenderer>();
        m_NearbyObjects = new List<int>();
    }

    void OnTriggerEnter(Collider i_Other)
    {
        if (i_Other.CompareTag("Object"))
            m_NearbyObjects.Add(i_Other.GetInstanceID());
    }
    
    void OnTriggerExit(Collider i_Other)
    {
        if (i_Other.CompareTag("Object") && m_NearbyObjects.Contains(i_Other.GetInstanceID()))
            m_NearbyObjects.Remove(i_Other.GetInstanceID());
    }

    // Update is called once per frame
    void Update()
    {
        CanBePlaced =  m_NearbyObjects.Count == 0;
        m_Mesh.material.color = CanBePlaced ? m_CorrectColor : m_WrongColor;
        
    }
}
