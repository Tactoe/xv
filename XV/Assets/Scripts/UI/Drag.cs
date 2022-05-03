using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Drag : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    Canvas m_Canvas;
    void Start()
    {
        m_Canvas = GetComponentInParent<Canvas>();
    }

    public void OnPointerDown(PointerEventData i_EventData)
    {
    }

    public void OnBeginDrag(PointerEventData i_EventData)
    {
    }
    
    public void OnEndDrag(PointerEventData i_EventData)
    {
    }

    public void OnDrag(PointerEventData i_EventData)
    {
        (transform as RectTransform).anchoredPosition += i_EventData.delta / m_Canvas.scaleFactor;
    }
}
