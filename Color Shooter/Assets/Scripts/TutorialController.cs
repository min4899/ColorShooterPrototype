using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour {

    public GameObject tutorialTextParent;

    public GameObject[] enemyWaves;

    public GameObject[] popupTexts;

    private int wave;
    private int textIndex;
    private bool started;
    private Vector2 lastPos;

	// Use this for initialization
	void Start ()
    {
        started = true;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (!GameController.instance.gameOver)
        {
            if (wave == 0) // 1st: Move ship by swiping at the screen
            {
                // if player already moved
                if (Player.instance.transform.position.x != 0.0f && Player.instance.transform.position.y != 0.0f)
                {
                    popupTexts[0].SetActive(false);
                    wave++;
                    started = true;
                }
                else
                {
                    popupTexts[0].SetActive(true);
                    if (started)
                    {
                        lastPos = Player.instance.transform.position;
                        started = false;
                    }
                    Vector2 curPos = Player.instance.transform.position;
                    if (curPos != lastPos)
                    {
                        popupTexts[0].SetActive(false);
                        wave++;
                        started = true;
                    }
                    lastPos = curPos;
                }
            }
            else if (wave == 1) // 2nd: Get hit to change colors and shoot
            {
                popupTexts[1].SetActive(true);
                if (started)
                {
                    Instantiate(enemyWaves[0]);
                    started = false;
                }
                if (Player.instance.colorState == 1) // Color switched to Red, remove text
                {
                    popupTexts[1].SetActive(false);
                }
                if (GameController.instance.enemiesKilled == 3) // killed enemies, move to next wave
                {
                    wave++;
                    started = true;
                }
            }
            else if (wave == 2) // 3rd: Switching to different colors
            {
                popupTexts[2].SetActive(true);
                if (started)
                {
                    Instantiate(enemyWaves[1]);
                    started = false;
                }
                if (Player.instance.colorState == 2) // Color switched to Red, remove text
                {
                    popupTexts[2].SetActive(false);
                }
                if (GameController.instance.enemiesKilled == 10) // killed enemies, move to next wave
                {
                    wave++;
                    started = true;
                }
            }
            else if (wave == 3) // 1st shield intro
            {
                popupTexts[3].SetActive(true);
                if (started)
                {
                    Instantiate(enemyWaves[2]);
                    started = false;
                }
                if (Player.instance.CheckShield() == false)
                {
                    popupTexts[4].SetActive(true);
                    popupTexts[5].SetActive(false);
                }
                //if (Player.instance.CheckShield() == true)
                else
                {
                    popupTexts[5].SetActive(true);
                    popupTexts[4].SetActive(false);
                }
                if (GameController.instance.enemiesKilled == 24) // killed enemies, move to next wave
                {
                    wave++;
                    started = true;
                    popupTexts[3].SetActive(false);
                }
            }
            else if (wave == 4) // 2nd shield practice
            {
                if (started)
                {
                    Instantiate(enemyWaves[3]);
                    started = false;
                }
                if (Player.instance.CheckShield() == false)
                {
                    popupTexts[4].SetActive(true);
                    popupTexts[5].SetActive(false);
                }
                //if (Player.instance.CheckShield() == true)
                else
                {
                    popupTexts[5].SetActive(true);
                    popupTexts[4].SetActive(false);
                }
                if (GameController.instance.enemiesKilled == 27) // killed enemies, move to next wave
                {
                    wave++;
                    started = true;
                    popupTexts[4].SetActive(false);
                    popupTexts[5].SetActive(false);
                }
            }
            else if (wave == 5)
            {
                GameObject currentWave = null;
                if (started)
                {
                    currentWave = Instantiate(enemyWaves[4]);
                    started = false;
                }
                if (GameObject.FindGameObjectsWithTag("Enemy").Length > 0 && currentWave == null)
                {
                    wave++;
                    started = true;
                }
            }
        }
        else // gameOver true
        {
            tutorialTextParent.SetActive(false);
        }
	}
}
