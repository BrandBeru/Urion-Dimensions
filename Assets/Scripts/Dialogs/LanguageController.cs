using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LanguageController : MonoBehaviour
{
    private TextAsset textDialogs;
    private string[] data;

    private string languageSystem;

    public string file;
    void Awake()
    {
        textDialogs = Resources.Load<TextAsset>(file+" - Urion");
        data = textDialogs.text.Split('\n');
    }
    public string GetDialog(char split, int dialog)
    {
        languageSystem = PlayerPrefs.GetString("LANGUAGE");
        string[] languages = data[dialog].Split(split);
        switch (languageSystem)
        {
            case "Español":
                return languages[1];
            case "English":
                return languages[3];
            default:
                return "Este mensaje no deberia mostrarse jamas!";
        }
    }
}
