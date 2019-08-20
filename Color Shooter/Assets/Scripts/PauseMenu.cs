using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PauseMenu : MonoBehaviour {

    public static PauseMenu instance;
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;
    public TextMeshProUGUI countDownText;
    public GameObject hud;
    //public GameObject player;

    //private PlayerControl playerControl;
    private Coroutine currentCountDown;

    void Awake()
    {
        instance = this;
    }

    void Start () {
        //Resume();
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        //test
        //if (player != null)
        /*
        if (Player.instance != null)
        {
            playerControl.enabled = true;
        }
        */
    }
	
	// Update is called once per frame
	void Update () {
#if UNITY_STANDALONE || UNITY_EDITOR
        //if (Input.GetKeyDown(KeyCode.Escape) && !GameController.instance.gameOver)
        if (Input.GetKeyDown(KeyCode.Escape) && Player.instance != null)
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
            //if(!GameController.instance.gameOver) // If game is not over, can still pause
            if (Player.instance != null && hud.activeSelf) // player still alive
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
            //if (!GameController.instance.gameOver) // If game is not over, can still pause
            if (Player.instance != null && hud.activeSelf) // player still alive
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
            /*
            if (Player.instance != null)
            {
                playerControl.enabled = true;
            }
            */
            currentCountDown = StartCoroutine(CountDownResume());
        }
#endif

#if UNITY_STANDALONE || UNITY_EDITOR
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        /*
        if (Player.instance != null)
        {
            playerControl.enabled = true;
        }
        */
        currentCountDown = StartCoroutine(CountDownResume());
#endif
    }

    IEnumerator CountDownResume()
    {
        Time.timeScale = 0.45f;
        countDownText.enabled = true;
        countDownText.text = "3";
        yield return new WaitForSeconds(0.45f);
        countDownText.text = "2";
        yield return new WaitForSeconds(0.45f);
        countDownText.text = "1";
        yield return new WaitForSeconds(0.45f);
        countDownText.enabled = false;
        Time.timeScale = 1f;
    }

    public void Pause()
    {
        if (currentCountDown != null) // if game is paused during the countdown, cancel it
        {
            StopCoroutine(currentCountDown);
        }
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        /*
        if (Player.instance != null)
        {
            playerControl.enabled = false;
        }
        */
    }

    public void Restart()
    {
        if (currentCountDown != null) // if game is in countdownresume, cancel it
        {
            StopCoroutine(currentCountDown);
        }
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
        if (currentCountDown != null) // if game is in countdownresume, cancel it
        {
            StopCoroutine(currentCountDown);
        }
        //if (Input.touchCount == 1)
        //{
        Time.timeScale = 1f;
        GameIsPaused = false;
        AdManager.instance.ShowRegularAd(); // test
        SceneManager.LoadScene("_MainMenu");
        //}
    }

    public void ShowAd()
    {
        AdManager.instance.ShowRewardedAd();
    }

    
    public void Respawn()
    {
        Time.timeScale = 1f;
        GameController.instance.RespawnPlayer();
        //Resume(); // give slow motion upon respawn
        pauseMenuUI.SetActive(false);
        GameIsPaused = false;
        currentCountDown = StartCoroutine(CountDownResume());
    }
    

    /*
    public IEnumerator Respawn()
    {
        GameController.instance.RespawnPlayer();
        yield return new WaitForSeconds(0.5f);
        Resume(); // give slow motion upon respawn
    }
    */

    public void GiveUp()
    {
        Time.timeScale = 1f;
        GameController.instance.GameOver();
    }
}
