using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    //Game Manager
    public string weaponName;
    public string weaponType;
    public Sprite weaponSprite;
    public float powerAmount;
    public float speed;
    public int level;

    //Store
    public int price;
    public int experience;
    public bool isBought;

    public void Bought()
    {
        isBought= true;
    }
    public bool IsBought()
    {
        isBought = !(PlayerPrefs.GetString(weaponName).Equals(""));
        return (price == 0 || isBought);
    }
    public bool CanBuy(int coins, int experienceNeeded)
    {
        isBought = !(PlayerPrefs.GetString(weaponName).Equals(""));
        Debug.Log("Is bought: " + isBought);
        return (coins >= this.price && experienceNeeded >= this.experience && !isBought );
    }
}
