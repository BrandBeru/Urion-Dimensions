using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public AudioMixer audioMixer;

    //Audio
    public Slider masterSLD;
    public TextMeshProUGUI masterLevel;
    public Slider musicSLD;
    public TextMeshProUGUI musicLevel;
    public Slider soundSLD;
    public TextMeshProUGUI soundLevel;

    private void Start()
    {        
        InitVolume();
    }
    public void InitVolume()
    {
        if (PlayerPrefs.GetInt("Volume") == 1)
        {
            masterSLD.value = PlayerPrefs.GetFloat("Master_PERCENT");
            musicSLD.value = PlayerPrefs.GetFloat("BGM_PERCENT");
            soundSLD.value = PlayerPrefs.GetFloat("SFX_PERCENT");
        }
        else            
            Apply();

        ChangeVolume();
    }
    public void Apply()
    {
        PlayerPrefs.SetFloat("Master_PERCENT", masterSLD.value);
        PlayerPrefs.SetFloat("BGM_PERCENT", musicSLD.value);
        PlayerPrefs.SetFloat("SFX_PERCENT", soundSLD.value);

        PlayerPrefs.SetInt("Volume", 1);

        InitVolume();
    }
    public void Back()
    {
        InitVolume();
    }
    private void ChangeVolume()
    {
        float decibelios = musicSLD.value == 0 ? -80 : (20 * Mathf.Log10(musicSLD.value / 100));
        audioMixer.SetFloat("BGM", decibelios);
        musicLevel.text = musicSLD.value.ToString();

        decibelios = soundSLD.value == 0 ? -80 : (20 * Mathf.Log10(soundSLD.value / 100));
        audioMixer.SetFloat("SFX", decibelios);
        soundLevel.text = soundSLD.value.ToString();

        decibelios = masterSLD.value == 0 ? -80 : (20 * Mathf.Log10(masterSLD.value / 100));
        audioMixer.SetFloat("Master", decibelios);
        masterLevel.text = masterSLD.value.ToString();
    }
    public void ChangeVolume(string channel)
    {
        float percent = 0;
        float decibelios = 0f;
        if (channel == "BGM")
        {
            percent = musicSLD.value;
            musicLevel.text = (percent).ToString();
            decibelios = percent == 0 ? -80 : (20 * Mathf.Log10(percent / 100));
        }
        else if (channel == "SFX")
        {
            percent = soundSLD.value;
            soundLevel.text = (percent).ToString();
            decibelios = percent == 0 ? -80 : (20 * Mathf.Log10(percent / 100));
        }
        else if (channel == "Master")
        {
            percent = masterSLD.value;
            masterLevel.text = (percent).ToString();
            decibelios = percent == 0 ? -80 : (20 * Mathf.Log10(percent / 100));
        }
        audioMixer.SetFloat(channel, decibelios);
    }
}
