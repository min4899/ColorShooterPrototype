using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using BayatGames.SaveGameFree;

public class LevelCleared : MonoBehaviour
{
    public GameObject mainMenuButton;
    public GameObject infoButton;
    public TextMeshProUGUI finalScore;
    public TextMeshProUGUI killCount;
    public GameObject stars;
    public GameObject hud;

    private int score;
    private int enemiesKilled;
    private int enemyCount;
    private float percentKilled;
    private int deaths;
    private int health;
    private int maxHealth;
    private int finalStars;

    private GameObject currentCanvas;

    // Use this for initialization
    void Start ()
    {
        currentCanvas = gameObject.transform.GetChild(0).gameObject;
        currentCanvas.SetActive(true); // set level cleared screen as active on start
        gameObject.transform.GetChild(1).gameObject.SetActive(false); // set star rating info screen as nonactive on start
        finalScore.enabled = false;
        killCount.enabled = false;
        stars.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetValues()
    {
        score = GameController.instance.score;
        enemiesKilled = GameController.instance.enemiesKilled;
        enemyCount = GameController.instance.enemyCount;
        deaths = GameController.instance.deaths;
        health = Player.instance.health;
        maxHealth = GameController.instance.maxHealth;

        percentKilled = (float)enemiesKilled / (float)enemyCount;

        if (percentKilled == 1f && deaths == 0 && health == maxHealth)
        {
            finalStars = 4;
            stars.transform.GetChild(3).gameObject.SetActive(true);
        }
        else if (percentKilled >= 0.9f && deaths == 0 && health <= maxHealth)
        {
            finalStars = 3;
            stars.transform.GetChild(2).gameObject.SetActive(true);
        }
        else if (percentKilled >= 0.75f)
        {
            finalStars = 2;
            stars.transform.GetChild(1).gameObject.SetActive(true);
        }
        else
        {
            finalStars = 1;
            stars.transform.GetChild(0).gameObject.SetActive(true);
        }

        /*
        // Save value
        int levelIndex = SceneManager.GetActiveScene().buildIndex - 1;
        if (MainMenu.levels != null)
        {
            MainMenu.levels[levelIndex].cleared = true;
            if (score > MainMenu.levels[levelIndex].bestScore)
            {
                MainMenu.levels[levelIndex].bestScore = score;
            }
            if (finalStars > MainMenu.levels[levelIndex].bestStars)
            {
                MainMenu.levels[levelIndex].bestStars = finalStars;
            }
            SaveGame.Save<Level[]>("levels", MainMenu.levels);

            Debug.Log("Saved stats for Scene " + SceneManager.GetActiveScene().buildIndex.ToString() + " (levels array index:" + (levelIndex).ToString() + ")");
        }
        else
        {
            Debug.Log("Stats array not found. No score is saved.");
        }
        */
    }

    public void SaveStats()
    {
        // Save value
        int levelIndex = SceneManager.GetActiveScene().buildIndex - 1;
        if (MainMenu.levels != null)
        {
            MainMenu.levels[levelIndex].cleared = true;
            if (score > MainMenu.levels[levelIndex].bestScore)
            {
                MainMenu.levels[levelIndex].bestScore = score;
            }
            if (finalStars > MainMenu.levels[levelIndex].bestStars)
            {
                MainMenu.levels[levelIndex].bestStars = finalStars;
            }
            SaveGame.Save<Level[]>("levels", MainMenu.levels);

            Debug.Log("Saved stats for Scene " + SceneManager.GetActiveScene().buildIndex.ToString() + " (levels array index:" + (levelIndex).ToString() + ")");
        }
        else
        {
            Debug.Log("Stats array not found. No score is saved.");
        }
    }

    public IEnumerator LevelClearedSequence()
    {
        // delay before bringing up last screen
        //yield return new WaitForSeconds(endLevelDelay);

        hud.SetActive(false);

        // set the text for scores and kill count
        finalScore.text = score.ToString();
        killCount.text = enemiesKilled.ToString() + " / " + enemyCount;

        // Turn off music
        MusicManager.instance.Pause();

        // Initially disable the texts and button
        mainMenuButton.SetActive(false);
        infoButton.SetActive(false);
        finalScore.enabled = false;
        killCount.enabled = false;

        GameController.instance.gameOver = true;
        Player.instance.GetComponent<Player>().enabled = false; // disable player controls and shooting
        Player.instance.GetComponent<PlayerControl>().enabled = false;
        gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        AudioManager.instance.PlaySound("Text_PopUp");
        finalScore.enabled = true;
        yield return new WaitForSeconds(1);
        AudioManager.instance.PlaySound("Text_PopUp");
        killCount.enabled = true;
        yield return new WaitForSeconds(1);
        AudioManager.instance.PlaySound("Text_PopUp");
        stars.SetActive(true);
        yield return new WaitForSeconds(1);
        mainMenuButton.SetActive(true);
        infoButton.SetActive(true);
    }

    public void LevelClearedMainMenuButton()
    {
        Time.timeScale = 1f;
        PauseMenu.GameIsPaused = false;
        AdManager.instance.ShowRegularAd();
        SceneManager.LoadScene("_MainMenu");
    }

    public void GotoOption(GameObject selection)
    {
        selection.SetActive(true); // set selected menu active
        currentCanvas.SetActive(false); 
        currentCanvas = selection;
    }
}
