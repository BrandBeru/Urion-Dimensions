using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menus : MonoBehaviour
{
    [SerializeField] private Animator anim;

    public string[] languages;
    private int index;

    //Video
    //Resolution
    public TextMeshProUGUI resolutionsTXT;
    private Resolution[] resolutions;
    private Resolution actResolution;
    int resPosition;
        //Full screen
    public TextMeshProUGUI fullScreenTXT;
    private bool fullScreen;
        //V-Sync
    public TextMeshProUGUI vsyncTXT;
    private bool vsync;
        //Quality
    public TextMeshProUGUI qualityTXT;
    private int level;
        

    //Game
    //Language
    public TextMeshProUGUI languageTXT;
    public LanguageController languageTexts;

    private void Start()
    {
        resolutions = Screen.resolutions;
        InitValues();
    }
    public void ChangeFullScreen()
    {
        fullScreen = !fullScreen;
        if (fullScreen)
            fullScreenTXT.text = "full screen: on";
        else
            fullScreenTXT.text = "full screen: off";
    }
    public void ChangeVSync()
    {
        vsync = !vsync;
        if (vsync)
            vsyncTXT.text = "v-sync: on";
        else
            vsyncTXT.text = "v-sync: off";
    }
    public void ChangeLanguage()
    {
        index++;
        if (index > languages.Length - 1)
            index = 0;
        else if (index < 0)
            index = languages.Length - 1;

        PlayerPrefs.SetString("LANGUAGE", languages[index]);
        PlayerPrefs.SetInt("LANGUAGE_INDEX", index);
        Debug.Log("Language game: " + languages[index]);
    }
    public void ChangeResolution()
    {
        resPosition++;
        if (resPosition > resolutions.Length-1)
            resPosition = 0;
        actResolution = resolutions[resPosition];
        resolutionsTXT.text = "resolution: " + actResolution.width + " x " + actResolution.height + " @ " + actResolution.refreshRate;
    }
    public void ChangeQuality()
    {
        level = QualitySettings.GetQualityLevel();
        qualityTXT.text = "quality: " + QualitySettings.names[level];
        QualitySettings.SetQualityLevel(level+1);
    }
    public void ApplyChanges()
    {

    }
    public void InitValues()
    {
        if (PlayerPrefs.GetString("LANGUAGE").Equals(""))
            PlayerPrefs.SetString("LANGUAGE", languages[0]);
        else
        {
            Debug.Log("Already Created");
            index = PlayerPrefs.GetInt("LANGUAGE_INDEX")-1;
            ChangeLanguage();
        }
        actResolution = resolutions[resolutions.Length - 1];
        resolutionsTXT.text = "resolution: " + actResolution.width + " x " + actResolution.height + " @ " + actResolution.refreshRate;
        ChangeVSync();
        ChangeFullScreen();
        ChangeQuality();
    }
    public void ChangeScene(float delay)
    {
        StartCoroutine(Retarded(delay));
    }
    private IEnumerator Retarded(float retarded)
    {
        yield return new WaitForSeconds(retarded);
        Loader.Charging("SaksafnDimension");
    }
    public void DeleteAll()
    {
        PlayerPrefs.DeleteAll();
    }
}
