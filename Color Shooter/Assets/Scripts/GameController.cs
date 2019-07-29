using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour {

    public static GameController instance;

    public TextMeshProUGUI countDownText;
    public GameObject level;
    public TextMeshProUGUI scoreDisplay;
    public GameObject gameOverScreen;
    public GameObject levelClearedScreen;
    public TextMeshProUGUI finalScore;
    public TextMeshProUGUI killCount;
    public float endLevelDelay;

    public int score;
    public int enemiesKilled;
    public int enemyCount;

    [HideInInspector] public bool gameOver;
    [HideInInspector] public int lastColorDestroyed;
    [HideInInspector] public int colorCombo;

    // Use this for initialization
    void Awake()
    {
        instance = this;
        gameOver = false;
        lastColorDestroyed = 0;
        colorCombo = 0;
    }

    void Start () {
        enemiesKilled = 0;
        enemyCount = 0;
        score = 0;
        countDownText.enabled = true;
        gameOverScreen.SetActive(false);
        levelClearedScreen.SetActive(false);
        UpdateScore();
        StartCoroutine(GetReady());
    }

    void Update()
    {
        if(Player.instance == null)
        {
            StartCoroutine(GameOver());
        }
    }

    IEnumerator GetReady()
    {
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
        yield return new WaitForSeconds(0.5f);
        level.SetActive(true);
    }

    public void UpdateScore()
    {
        scoreDisplay.text = score.ToString();
    }

    IEnumerator GameOver()
    {
        gameOver = true;
        yield return new WaitForSeconds(1.5f);
        gameOverScreen.SetActive(true);
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

    IEnumerator LevelCleared()
    {
        // delay before bringing up last screen
        yield return new WaitForSeconds(endLevelDelay);

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
}
