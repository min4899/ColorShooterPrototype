using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckForEnemies : MonoBehaviour {
    public float countDown = 2;

    private float currentCount;
    private bool continueCount;

	// Use this for initialization
	void Start ()
    {
        currentCount = 0;
        if(GameObject.FindWithTag("Boss") == null) // if no bosses are found, use this script for finishing level
        {
            continueCount = true;
        }
        else // boss object found, use boss(es)'s own script for ending level
        {
            continueCount = false;
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (continueCount)
        {
            // reset counter if enemies are still alive
            if (GameObject.FindWithTag("Enemy") != null)
            {
                currentCount = 0;
            }
            currentCount += Time.deltaTime;
            if (Player.instance != null && currentCount >= countDown) // player is still alive by the end of countdown.
            {
                GameController.instance.EndLevel();
                continueCount = false;
            }
        }
	}
}
