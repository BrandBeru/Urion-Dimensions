using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public AudioClip audioClip;
    public AudioSource controller;

    public void Play()
    {
        controller.Stop();
        controller.PlayOneShot(audioClip);
    }
    public void Play(AudioClip custom)
    {
        controller.Stop();
        controller.PlayOneShot(custom);
    }
    public void Stop()
    {
        controller.Stop();
    }
}
