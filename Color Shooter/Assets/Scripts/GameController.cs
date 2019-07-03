using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour {

    public TextMeshProUGUI countDownText;
    public GameObject level;
    public GameObject gameOverScreen;


    // Use this for initialization
    void Start () {
        countDownText.enabled = true;
        gameOverScreen.SetActive(false);
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

    IEnumerator GameOver()
    {
        yield return new WaitForSeconds(1.5f);
        gameOverScreen.SetActive(true);
    }
}
