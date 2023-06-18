using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MiniGamesManager : MonoBehaviour
{
    public static MiniGamesManager instance;
    public GameState currentGameState = GameState.DIALOG;

    public LanguageController language;

    public PlayerMovement player;
    public EnemyBehaviour enemy;
    public MyEventSystem optionsEvents;
    public MyEventSystem controlsEvents;

    [SerializeField] private Slider playerHPSLD;
    [SerializeField] private Slider enemyHPSLD;
    [SerializeField] private TextMeshProUGUI playerNameTXT;
    [SerializeField] private TextMeshProUGUI enemyNameTXT;

    [SerializeField] private TextMeshProUGUI messageText;

    private float playerHP;
    public float maxPlayerHealth;
    private float enemyHP;
    public float maxEnemyHealth;
    
    private Queue<char> chars= new Queue<char>();

    private void Awake()
    {
        if (instance == null)
            instance = this;

        playerHP = maxPlayerHealth;
        enemyHP = maxEnemyHealth;

        playerHPSLD.maxValue = maxPlayerHealth;
        enemyHPSLD.maxValue = maxEnemyHealth;
        playerHPSLD.value = playerHP;
        enemyHPSLD.value = enemyHP;
    }
    private void Start()
    {
        StopGame(7);
    }
    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (currentGameState == GameState.STOP)
                StartGame();
            else if(currentGameState == GameState.START)
                StopGame(8);
        }
    }
    public void Fight()
    {
        if (currentGameState == GameState.STOP)
            StartGame();
        else if (currentGameState == GameState.GAME_OVER)
            Loader.Charging(SceneManager.GetActiveScene().name);
    }
    public float GetEnemyHealth()
    {
        return enemyHP;
    }
    public float GetPlayerHealth()
    {
        return playerHP;
    }
    public void TakeDamage(bool isPlayer)
    {
        if (isPlayer)
        {
            playerHP -= (enemy.damage/player.resistence);
            if (playerHP <= 0)
                GameOver();
            else
                SetText(6);
            playerHPSLD.value = playerHP;
            CameraMovement.instance.Movement(3, 3, 0.3f);
        }
        else
        {
            enemyHP-= (player.damage/enemy.resistence);
            if(enemyHP <= 0)
                GamePass();
            else
                SetText(5);
            enemyHPSLD.value = enemyHP;
            CameraMovement.instance.Movement(1, 1, 0.1f);
        }
    }
    public void PointAmount(bool isPlayer)
    {

    }
    public void StartGame()
    {
        Time.timeScale = 1;
        SetText(10);
        SetGame(GameState.START);
        optionsEvents.enabled = false;
        controlsEvents.enabled = true;
    }
    public void StopGame(int msg)
    {
        SetGame(GameState.STOP);

        SetText(msg);
        optionsEvents.enabled = true;
        controlsEvents.enabled = false;
        Time.timeScale = 0;
    }
    public void GameOver()
    {
        SetText(3);
        SetGame(GameState.GAME_OVER);
        optionsEvents.enabled = true;
        controlsEvents.enabled = false;
        Time.timeScale = 0;
    }
    public void GamePass()
    {
        enemyHP = 0;
        enemyHPSLD.value = enemyHP;
        SetGame(GameState.GAME_PASS);
        SetText(4);
        Time.timeScale = 0;
    }
    private void SetGame(GameState newGameStage)
    {
        this.currentGameState = newGameStage;
    }
    string dialog;
    public void SetText(int msg)
    {
        dialog = language.GetDialog('\u0022', msg);
        chars.Clear();
        foreach (char c in dialog.ToCharArray())
        {
            chars.Enqueue(c);
            Debug.Log(c);
        }
        messageText.enabled = true;
        messageText.text = "";
        StartCoroutine(CharSequence());
    }
    private IEnumerator CharSequence()
    {
        for (int i = 0; i < dialog.Length; i++)
        {
            messageText.text += chars.Dequeue();
            yield return new WaitForEndOfFrame();
        }
    }
}
