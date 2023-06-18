using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Players : MonoBehaviour
{
    public Animator playerAnim;
    public Sprite headSprite;
    public Weapon[] weapons;

    public float speed;
    public float strenght;
    public int experience;
    public int level;

    //UI Settings
    public Color playerColor;

    public void UpdateCharacter()
    {
        weaponLevel = PlayerPrefs.GetInt(name + "_WEAPON_LEVEL");
        GameManager.instance.weaponSelected = weapons[weaponLevel];
    }

    //Weapon
    public int weaponLevel = 0;

    private bool isEnable;

    public bool IsEnable()
    {
        return isEnable;
    }
}
