using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Texts : MonoBehaviour
{
    public int idText;
    public LanguageController language;

    string text;
    private void Start()
    {
        text = language.GetDialog(',', idText);
    }
    private void Update()
    {
            ChangeLanguage();
    }
    public void ChangeLanguage()
    {
        text = language.GetDialog(',', idText);
        gameObject.GetComponent<TextMeshProUGUI>().text = text;
    }
}
