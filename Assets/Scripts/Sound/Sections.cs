using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sections : MonoBehaviour
{
    public AudioSource audioSource;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            audioSource.Play();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            audioSource.Pause();
        }
    }
}
