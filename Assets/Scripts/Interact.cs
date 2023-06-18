using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
    public int action;
    public void ShowIntercat()
    {
        GameManager.instance.canvasAnim.SetTrigger("Interact");
    }
    public void Action()
    {
        if(action == 1)
            GameManager.instance.UpdateStoreMenu();
        else if(action == 2)
            GameManager.instance.npcIntercating.StartDialog();
    }
}
