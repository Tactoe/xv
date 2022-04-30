using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Drag : MonoBehaviour
{
    [SerializeField]
    Canvas m_Canvas;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void DragHandler(BaseEventData i_Event)
    {
        PointerEventData pointer = (PointerEventData)i_Event;
        Vector2 offset = (Vector2)transform.position - pointer.position;

        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)m_Canvas.transform,
            pointer.position,
            m_Canvas.worldCamera,
            out pos
        );

        transform.position = m_Canvas.transform.TransformPoint(pos);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
