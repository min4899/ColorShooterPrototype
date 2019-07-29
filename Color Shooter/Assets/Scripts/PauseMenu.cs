using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PauseMenu : MonoBehaviour {

    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;
    public TextMeshProUGUI countDownText;
    //public GameObject player;

    private PlayerControl playerControl;

    void Start () {
        //playerControl = player.GetComponent<PlayerControl>();
        playerControl = Player.instance.GetComponent<PlayerControl>();
        Resume();
    }
	
	// Update is called once per frame
	void Update () {
#if UNITY_STANDALONE || UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Escape))
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
#endif
    }
    
    // COMMENT START: FOR TESTING
#if UNITY_IOS || UNITY_EDITOR
    private void OnApplicationFocus(bool focus)
    {
        if (focus == false)
        {
            if(!GameController.instance.gameOver) // If game is not over, can still pause
                Pause();
            MusicManager.instance.Pause();
        }
        else
        {
            MusicManager.instance.Unpause();
        }
    }
#endif

#if UNITY_ANDROID || UNITY_EDITOR
    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            if (!GameController.instance.gameOver) // If game is not over, can still pause
                Pause();
            MusicManager.instance.Pause();
        }
        else
        {
            MusicManager.instance.Unpause();
        }
    }
#endif
    // COMMENT END: FOR TESTING 

    public void Resume()
    {
#if UNITY_IOS || UNITY_ANDROID
        if (Input.touchCount == 1)
        {
            pauseMenuUI.SetActive(false);
            Time.timeScale = 1f;
            GameIsPaused = false;
            //test
            //if (player != null)
            if (Player.instance != null)
            {
                playerControl.enabled = true;
            }
        }
#endif

#if UNITY_STANDALONE || UNITY_EDITOR
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        //test
        //if (player != null)
        if (Player.instance != null)
        {
            playerControl.enabled = true;
        }
#endif

    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        //test
        //if (player != null)
        if (Player.instance != null)
        {
            playerControl.enabled = false;
        }
    }

    public void Restart()
    {
        //if (Input.touchCount == 1)
        //{
            Time.timeScale = 1f;
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
            //test
            GameIsPaused = false;
        //}
    }

    public void ReturnToMain()
    {
        //if (Input.touchCount == 1)
        //{
            Time.timeScale = 1f;
            GameIsPaused = false;
            SceneManager.LoadScene("_MainMenu");
        //}
    }
}
