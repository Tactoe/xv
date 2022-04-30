using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData : MonoBehaviour
{
    public float Weight;

    public void DisplayWindow()
    {
        EditWindow.Instance.EnableWindow(transform.parent.gameObject);
    }
}
