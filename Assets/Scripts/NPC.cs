using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField] private string npcName;
    [SerializeField] private Sprite npcSprite;

    [SerializeField] private int dialogID;

    [SerializeField] private bool isIntercative;
    public DialogsController typeDialog;

    public SpriteRenderer infoSprite;

    public bool finishedDialog;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            GameManager.instance.npcIntercating = this;
            if (isIntercative)
            {
                GameManager.instance.canvasAnim.SetTrigger("Interact");
                GameManager.instance.interactAction.action = 2;
            }
            else
                StartDialog();
        }
    }
    public void StartDialog()
    {
        typeDialog.ShowDialog(npcSprite, npcName, dialogID, transform, isIntercative);
    }
    public void StopDialog()
    {
        typeDialog.HideDialog(isIntercative);
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            GameManager.instance.canvasAnim.SetTrigger("PlayerUI");
            if (!isIntercative)
                StopDialog();
        }
    }
}
