using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData : MonoBehaviour
{
    public float Weight;
    public MeshRenderer[] Renderers;
    public Material[] Materials;

    public void DisplayWindow()
    {
        EditWindow.Instance.EnableWindow(transform.parent.gameObject);
    }
}
