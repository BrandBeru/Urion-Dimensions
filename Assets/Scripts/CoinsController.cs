using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinsController : MonoBehaviour
{
    [SerializeField] private int amount;
    [SerializeField] private bool isCoin;

    private int isColected = 0;
    private void Start()
    {
        isColected = PlayerPrefs.GetInt(gameObject.name);
        if(isColected != 0)
            Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!isCoin)
            PlayerPrefs.SetInt(gameObject.name, 1);
        CoinAmount();
    }
    private void CoinAmount()
    {
        if (isCoin)
        {
            GameManager.instance.coins += amount;
            
        }
        else
        {
            GameManager.instance.diamonds += amount;
        }
        GameManager.instance.UpdateView();
        Destroy(gameObject);
    }
}
