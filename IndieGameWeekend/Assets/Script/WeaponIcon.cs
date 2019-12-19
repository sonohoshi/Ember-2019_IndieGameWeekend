using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WeaponIcon : MonoBehaviour, IPointerDownHandler, IDragHandler,IPointerUpHandler
{
    public Vector2 pivot;
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position + pivot;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        pivot = eventData.position - (Vector2)transform.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
    }
}
