using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour {

    public static GameController instance;

    public TextMeshProUGUI countDownText;
    public GameObject level;
    public GameObject hud;
    public TextMeshProUGUI scoreDisplay;
    public TextMeshProUGUI multiplierDisplay;
    public GameObject continueScreen;
    public GameObject gameOverScreen;
    public GameObject levelClearedScreen;
    //public TextMeshProUGUI finalScore;
    //public TextMeshProUGUI killCount;
    public float endLevelDelay;

    public int score;
    public int enemiesKilled;
    public int enemyCount;

    public GameObject playerObject; // for respawning

    [HideInInspector] public bool gameOver;
    [HideInInspector] public int lastColorDestroyed;
    [HideInInspector] public int colorCombo;
    [HideInInspector] public int deaths;
    [HideInInspector] public int maxHealth;

    private TextMeshProUGUI playerHealthBar;
    private bool activeScreen; // used to prevent panels from repeating in update

    // Use this for initialization
    void Awake()
    {
        instance = this;
        gameOver = false;
        activeScreen = true;
        lastColorDestroyed = 0;
        colorCombo = 0;
        deaths = 0;
    }

    void Start () {
        playerHealthBar = Player.instance.healthBar; // test
        maxHealth = Player.instance.health; // Get the maximum health that player has at the start
        enemiesKilled = 0;
        enemyCount = 0;
        score = 0;
        countDownText.enabled = true;
        gameOverScreen.SetActive(false);
        levelClearedScreen.SetActive(false);
        hud.SetActive(false);
        UpdateScore();
        StartCoroutine(GetReady());
    }

    void Update()
    {
        /*
        if (Player.instance == null)
        {
            StartCoroutine(GameOver());
        }
        */      
        if (Player.instance == null && activeScreen)
        {
            hud.SetActive(false);
            if (deaths == 1) // 1st death, give continue chance
            {
                StartCoroutine(Continue());
                //Continue();
            }
            else if (deaths > 1) // more than 1 deaths, go to game over immediately
            {
                StartCoroutine(GameOver2());
            }
            else
            {
                StartCoroutine(GameOver2());
            }
        }
        
    }

    IEnumerator GetReady()
    {
        hud.SetActive(false);
        countDownText.enabled = true;
        countDownText.text = "3";
        yield return new WaitForSeconds(1);
        countDownText.text = "2";
        yield return new WaitForSeconds(1);
        countDownText.text = "1";
        yield return new WaitForSeconds(1);
        countDownText.text = "Go";
        yield return new WaitForSeconds(1);
        countDownText.enabled = false;
        hud.SetActive(true); // turn on hud
        yield return new WaitForSeconds(0.5f);
        level.SetActive(true); // start level
    }

    public void UpdateScore()
    {
        scoreDisplay.text = score.ToString();
    }

    public void UpdateMultiplier()
    {
        switch(lastColorDestroyed)
        {
            case 0:
                multiplierDisplay.color = Color.white;
                break;
            case 1:
                //multiplierDisplay.color = Color.red;
                multiplierDisplay.color = new Color(1f, 0.2745f, 0.2745f, 1f);
                break;
            case 2:
                //multiplierDisplay.color = Color.yellow;
                multiplierDisplay.color = new Color(1f, 0.9803f, 0.3137f, 1f);
                break;
            case 3:
                //multiplierDisplay.color = Color.green;
                multiplierDisplay.color = new Color(0.3529f, 1f, 0.3529f, 1f);
                break;
            default:
                break;
        }
        multiplierDisplay.text = "x" + colorCombo.ToString();
    }

    //IEnumerator Continue() // continue screen pop up
    IEnumerator Continue()
    {
        activeScreen = false;
        float timer = 1f;
        for(int i = 0; i < 5; i++) // slow down time to stop
        {
            Time.timeScale = timer;
            timer -= 0.2f;
            yield return new WaitForSeconds(0.15f);
        }
        Time.timeScale = 0.0f;
        continueScreen.SetActive(true);
        Debug.Log("Player died once, prompting continue");
    }

    public void RespawnPlayer() // for 'yes' button on the continue screen, respawn player and set variables
    {
        AudioManager.instance.PlaySound("Player_Respawn");
        GameObject player = Instantiate(playerObject);
        Player.instance = player.GetComponent<Player>();
        Player.instance.healthBar = playerHealthBar;
        continueScreen.SetActive(false);
        hud.SetActive(true);
        activeScreen = true;
        Debug.Log("Player succesfully respawned.");
    }

    public void GameOver() // use after player chooses 'no' in the continue screen
    {
        activeScreen = false;
        continueScreen.SetActive(false);
        gameOverScreen.SetActive(true);
        Debug.Log("Player chose to give up, Game Over.");
    }

    IEnumerator GameOver2() // if player already died more than once (used continue), immediately game over
    {
        activeScreen = false;
        continueScreen.SetActive(false);
        gameOver = true;
        yield return new WaitForSeconds(1.5f);
        gameOverScreen.SetActive(true);
        Debug.Log("No Respawns left. GameOver.");
    }


    public void EndLevel()
    {
        // Delete each enemy in the game
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(GameObject enemy in enemies)
        {
            enemy.GetComponent<Enemy>().Destruction();
        }
        // Destroy every enemy shots
        GameObject[] greenShots = GameObject.FindGameObjectsWithTag("EnemyGreenShot");
        foreach (GameObject shot in greenShots)
        {
            Destroy(shot);
        }
        GameObject[] yellowShots = GameObject.FindGameObjectsWithTag("EnemyYellowShot");
        foreach (GameObject shot in yellowShots)
        {
            Destroy(shot);
        }
        GameObject[] redShots = GameObject.FindGameObjectsWithTag("EnemyRedShot");
        foreach (GameObject shot in redShots)
        {
            Destroy(shot);
        }

        StartCoroutine(LevelCleared());
    }

    /*
    IEnumerator LevelCleared()
    {
        // delay before bringing up last screen
        yield return new WaitForSeconds(endLevelDelay);

        hud.SetActive(false);

        // set the text for scores and kill count
        finalScore.text = score.ToString();
        killCount.text = enemiesKilled.ToString() + " / " + enemyCount;

        // Turn off music
        MusicManager.instance.Pause();

        // Initially disable the texts and button
        GameObject mainMenuButton = levelClearedScreen.transform.Find("MenuButton").gameObject;
        mainMenuButton.SetActive(false);
        finalScore.enabled = false;
        killCount.enabled = false;

        gameOver = true;
        Player.instance.GetComponent<Player>().enabled = false; // disable player controls and shooting
        Player.instance.GetComponent<PlayerControl>().enabled = false;
        levelClearedScreen.SetActive(true);
        yield return new WaitForSeconds(1);
        finalScore.enabled = true;
        yield return new WaitForSeconds(1);
        killCount.enabled = true;
        yield return new WaitForSeconds(1);
        mainMenuButton.SetActive(true);
    }
    */

    IEnumerator LevelCleared()
    {
        yield return new WaitForSeconds(endLevelDelay);
        levelClearedScreen.SetActive(true);
        levelClearedScreen.GetComponent<LevelCleared>().SetValues();
        StartCoroutine(levelClearedScreen.GetComponent<LevelCleared>().LevelClearedSequence());
        levelClearedScreen.GetComponent<LevelCleared>().SaveStats();
    }
}
