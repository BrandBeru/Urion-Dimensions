using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Store : MonoBehaviour
{
    public Animator interact;
    bool isInteract;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" && !isInteract)
        {
            GameManager.instance.interactAction.action = 1;
            ShowStore();
            isInteract = true;
        }
    }
    private void ShowStore()
    {
        GameManager.instance.store = this;
        interact.SetTrigger("Interact");
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player" && isInteract)
        {
            interact.SetTrigger("PlayerUI");
            isInteract = false;
        }
    }
}
