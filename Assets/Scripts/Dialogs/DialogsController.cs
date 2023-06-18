using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogsController : MonoBehaviour
{
    public Image character;
    public TextMeshProUGUI charcater_name;
    public TextMeshProUGUI character_text;

    public Button next; 


    public GameObject dialogView;

    private SoundController soundController;
    private Queue<string> phrase;
    private List<char> c;
    private string[] sentenceParts;

    private bool isIntercat;

    public void ShowDialog(Sprite characterSprite, string charName, int dialogText, Transform position, bool interacter)
    {
        character_text.text = "";
        phrase = new Queue<string>();
        c = new List<char>();
        isIntercat= interacter;
        soundController = GameManager.instance.npcIntercating.GetComponent<SoundController>();
        if (!interacter)
        {
            Debug.Log("Mini Dialog");
            GameManager.instance.canvasAnim.SetTrigger("MiniDialog");
            Vector3 npcPosition = Camera.main.WorldToViewportPoint(position.position);
            dialogView.GetComponent<RectTransform>().anchoredPosition = new Vector3(npcPosition.x, npcPosition.y + 35, 0);
        }
        else
        {
            next.interactable = false;
            GameManager.instance.canvasAnim.SetTrigger("Dialog");
            GameManager.instance.player.SetCanMove(false);
        }
        character.sprite = characterSprite;
        charcater_name.text = charName;
        //text SHowing
        sentenceParts = GameManager.instance.language.GetDialog('\u0022', dialogText).Split(',');
        StartAnimationDialoge();

    }
    private void StartAnimationDialoge()
    {
        foreach(string individialPhrase in sentenceParts)
        {
            phrase.Enqueue(individialPhrase);
        }
        
        NextPhrase();
    }
    public void NextPhrase()
    {
        next.interactable = false;
        soundController.controller.pitch = 1;
        character_text.text = "";
        c.Clear();
        if (phrase.Count == 0)
        {
            Action();
        }
        else
        {
            string sentence = phrase.Dequeue();
            foreach (char ch in sentence.ToCharArray())
            {
                c.Add(ch);
            }
            StartCoroutine(CharSequence());
        }
    }
    private void Action()
    {
        GameManager.instance.interactAction.ShowIntercat();
        GameManager.instance.player.SetCanMove(true);
        GameManager.instance.npcIntercating.finishedDialog = true;
        GameManager.instance.npcIntercating.infoSprite.enabled = false;

    }
    private IEnumerator CharSequence()
    {
        yield return new WaitForSeconds(0.3f);
        soundController.Play();
        for(int i = 0; i < c.Count; i++)
        {
            character_text.text += c[i];
            yield return new WaitForEndOfFrame();
            soundController.controller.pitch += 0.01f;
        }
        next.interactable = true;
        next.Select();
    }
    public void HideDialog(bool interacter)
    {
        if (isIntercat)
            GameManager.instance.canvasAnim.SetTrigger("PlayerUI");
        else
            dialogView.GetComponent<RectTransform>().anchoredPosition = new Vector3(-500, 500, 0);
    }
}
