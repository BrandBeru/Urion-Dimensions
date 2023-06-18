using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonsController : MonoBehaviour,IPointerDownHandler, IPointerUpHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public bool isPressed = false;
    public bool isPressedDown = false;
    public bool isPressedUp = false;
    public bool isPressedClick = false;

    public bool isSelected;
    public void OnPointerClick(PointerEventData eventData)
    {
        StartCoroutine(ClickDelay());        
    }
    private IEnumerator ClickDelay()
    {
        isPressedClick = true;
        yield return new WaitForEndOfFrame();
        isPressedClick= false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
        isPressedUp = false;
        StartCoroutine(PressedDown());
    }
    private IEnumerator PressedDown()
    {
        isPressed = true;
        yield return new WaitForFixedUpdate();
        isPressed = false;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        isPressedUp = true;
        isPressed = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isPressed = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isPressed= false;
    }

}
