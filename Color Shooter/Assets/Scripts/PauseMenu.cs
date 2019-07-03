using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PauseMenu : MonoBehaviour {

    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;
    public TextMeshProUGUI countDownText;
    public GameObject player;

    private PlayerControl playerControl;
    private Transform playerLocation;

    // Use this for initialization
    void Start () {
        gameObject.SetActive(true);
        Resume();
        playerControl = player.GetComponent<PlayerControl>();
    }
	
	// Update is called once per frame
	void Update () {

		if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
	}

    public void Resume()
    {
        if(Input.touchCount == 1)
        {
            pauseMenuUI.SetActive(false);
            Time.timeScale = 1f;
            GameIsPaused = false;
            //test
            if (player != null)
            {
                playerControl.enabled = true;
            }
        }
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        //test
        if (player != null)
        {
            playerControl.enabled = false;
        }
        
    }

    public void Restart()
    {
        if (Input.touchCount == 1)
        {
            Time.timeScale = 1f;
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
            //test
            GameIsPaused = false;
        }
    }

    public void ReturnToMain()
    {
        if (Input.touchCount == 1)
        {
            Time.timeScale = 1f;
            GameIsPaused = false;
            SceneManager.LoadScene("MainMenu");
        }
    }
}
