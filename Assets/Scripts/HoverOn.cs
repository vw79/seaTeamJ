using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverOn : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
{
    public GameObject HoverObject;

    public void OnPointerEnter(PointerEventData eventData)
    {
        HoverObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HoverObject.SetActive(false);
    }
}
