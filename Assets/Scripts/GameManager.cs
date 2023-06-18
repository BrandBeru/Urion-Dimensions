using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //Scene view
    [HideInInspector] public static GameManager instance;

   public Animator canvasAnim;

    [SerializeField] private Slider healthSLD;
    [SerializeField] private Slider powerSLD;
    [SerializeField] private Slider experienceSLD;

    [SerializeField] private TextMeshProUGUI coinsTXT;
    [SerializeField] private TextMeshProUGUI diamondsTXT;
    [SerializeField] private TextMeshProUGUI playerLevelTXT;
    [SerializeField] private TextMeshProUGUI weaponLevelTXT;

    [SerializeField] private Image playerHeadIMG;
    [SerializeField] private Image weaponIMG;

    [HideInInspector] public NPC npcIntercating;

    public Interact interactAction;

    //Players
    public Players[] players;
    private int playerLevel;
    public Player_Movement player;

    public GameObject storeBoughtUI;
    //Texts
    [SerializeField] private TextMeshProUGUI storenameTXT;
    [SerializeField] private TextMeshProUGUI storelevelTXT;
    [SerializeField] private TextMeshProUGUI storepowerTXT;
    [SerializeField] private TextMeshProUGUI storevelTXT;
    [SerializeField] private TextMeshProUGUI storecoinsTXT;
    [SerializeField] private TextMeshProUGUI storexpTXT;
    //Sliders
    [SerializeField] private Slider storepowerSLD;
    [SerializeField] private Slider storevelSLD;
    [SerializeField] private Slider storexpSLD;
    //Image
    [SerializeField] private Image storeobjectImage;
    //BTN
    [SerializeField] private Button storebuyBTN;

    public LanguageController language;

    [HideInInspector] public Store store;

     public int coins;
    [HideInInspector] public int diamonds;

    //Menu validators;
    private bool playerSelectionActive;
    private bool storeActive;

    public int experience;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        InitilizeData();
    }
    public void DeleteAll()
    {
        PlayerPrefs.DeleteAll();
    }
    //Inputs from kewboard or joysticks
    private void Update()
    {
        
    }
    private void InitilizeData()
    {
        coins = PlayerPrefs.GetInt("COINS");
        diamonds = PlayerPrefs.GetInt("DIAMONDS");
        experience = PlayerPrefs.GetInt("EXPERIENCE");
        playerLevel = PlayerPrefs.GetInt("PLAYER_LEVEL");

        float x = PlayerPrefs.GetFloat("PLAYER-X"), y = PlayerPrefs.GetFloat("PLAYER-Y");
        if (x != 0 && y != 0)
        {
            Debug.Log("POsition");
            player.transform.position = new Vector3(x, y, 1);
        }

        playerSelected = players[playerLevel];
        playerSelected.weaponLevel = PlayerPrefs.GetInt(playerSelected.name+"_WEAPON_LEVEL");
        weaponSelected = playerSelected.weapons[playerSelected.weaponLevel];

        UpdateCharacter();
        UpdateView();
    }
    public void SaveState()
    {
        PlayerPrefs.SetInt("EXPERIENCE", experience);
        PlayerPrefs.SetInt("COINS", coins);
        PlayerPrefs.SetInt("DIAMONDS", diamonds);

        PlayerPrefs.SetFloat("Power", weaponSelected.powerAmount);
        PlayerPrefs.SetString("PLAYER_SELECTED", playerSelected.name);
        PlayerPrefs.SetInt("PLAYER_LEVEL", playerLevel);
        PlayerPrefs.SetString(weaponSelected.weaponName, weaponSelected.weaponName);
        PlayerPrefs.SetInt(playerSelected.name+"_WEAPON_LEVEL", playerSelected.weaponLevel);

        //Player position
        PlayerPrefs.SetFloat("PLAYER-X", player.transform.position.x);
        PlayerPrefs.SetFloat("PLAYER-Y", player.transform.position.y);


    }
    public void UpdateView()
    {
        coinsTXT.text = coins.ToString();
        diamondsTXT.text = diamonds.ToString();
        healthSLD.value = player.health;
        experienceSLD.value = experience;
    }
    public Weapon weaponSelected;
    public void ChangeWeapon()
    {
        Debug.Log("is bought: " + weaponSelected.IsBought());
            Debug.Log("Change to: " + weaponSelected.weaponName);

        playerSelected.weaponLevel = weaponSelected.level;
        weaponIMG.sprite = weaponSelected.weaponSprite;
        weaponLevelTXT.text = weaponSelected.level.ToString();
        powerSLD.value = weaponSelected.powerAmount;
        player.UpdateWeapon(weaponSelected.weaponSprite);
    }
    public Players playerSelected;
    public void ChangePlayer(int character)
    {
        playerLevel = character;
        playerSelected = players[playerLevel];
        playerSelectionActive = false;
        UpdateCharacter() ;
    }
    private void UpdateCharacter()
    {
        playerLevelTXT.text = playerSelected.level.ToString();
        playerHeadIMG.sprite = playerSelected.headSprite;
        playerSelected.UpdateCharacter();
        Debug.Log("WEapon Level: " + weaponSelected.level);
        player.UpdateSkills(playerSelected.speed);
        player.anim.runtimeAnimatorController = playerSelected.playerAnim.runtimeAnimatorController;
        Image headBackground = playerHeadIMG.transform.parent.gameObject.GetComponent<Image>();
        headBackground.color = playerSelected.playerColor;
        ChangeWeapon();
        UpdateView();
    }
    //Store
    [HideInInspector] public int pos;
    public void UpdateStoreMenu()
    {
        canvasAnim.SetTrigger("Store");
        pos = playerSelected.weaponLevel;
        if (pos > playerSelected.weapons.Length-1)
            pos = playerSelected.weapons.Length-1;
        else if (pos < 0)
            pos = 0;

        if (!playerSelected.weapons[pos].CanBuy(coins, experience))
        {
            storebuyBTN.enabled = false;
            storecoinsTXT.text = "Can't upgrade it";
        }
        else
        {
            storebuyBTN.enabled = true;
            storeBoughtUI.SetActive(false);
            storecoinsTXT.text = playerSelected.weapons[pos].price.ToString();
        }

        //UI Update
        storenameTXT.text = playerSelected.weapons[pos].weaponName;
        storelevelTXT.text = playerSelected.weapons[pos].level.ToString();
        storeobjectImage.sprite = playerSelected.weapons[pos].weaponSprite;

        storepowerTXT.text = playerSelected.weapons[pos].powerAmount.ToString();
        storepowerSLD.value = playerSelected.weapons[pos].powerAmount;
        storevelTXT.text = playerSelected.weapons[pos].speed.ToString();
        storevelSLD.value = playerSelected.weapons[pos].speed;
        storexpTXT.text = playerSelected.weapons[pos].experience.ToString();
        storexpSLD.value = playerSelected.weapons[pos].experience;
    }
    public void BuyObjectStore()
    {
        Weapon weaponShoping = playerSelected.weapons[pos];
        if (weaponShoping.CanBuy(coins, experience))
        {
            coins -= weaponShoping.price;
            experience -= weaponShoping.experience;
            weaponShoping.Bought();
            weaponSelected = weaponShoping;
            SaveState();
            UpdateCharacter();
            canvasAnim.SetTrigger("Interact");
            player.SetCanMove(true);
        }
        else
            storecoinsTXT.text = "INSUFFICENT";
    }
}
