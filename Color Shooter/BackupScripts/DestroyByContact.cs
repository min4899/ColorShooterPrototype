using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  Contact script for enemy and enemy bullets
/// </summary>
public class DestroyByContact : MonoBehaviour {

    //public GameObject explosion;
    //public GameObject playerExplosion;
    //public int scoreValue; // How much points to give for hitting

    //private GameController gameController; // FOR PC
    //private GameControllerAndroid gameController; // FOR ANDROID

    public int colorState;
    // no color = 0
    // yellow = 1
    // red = 1

    private GameObject player;
    private PlayerControl playercontrol;

    void Start()
    {
        /*
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }
        if (gameControllerObject == null)
        {
            Debug.Log("Cannot find 'GameController' script");
        }
        
        GameObject playerObject = GameObject.FindWithTag("Player");
        if(playerObject != null)
        {
            playercontrol = playerObject.GetComponent<PlayerControl>();
        }
        else
        {
            Debug.Log("Cannot find 'PlayerControl' script");
        }
        */
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Boundary") || other.CompareTag("Enemy") || other.CompareTag("EnemyRedShot") || other.CompareTag("EnemyYellowShot") )
        {
            return;
        }

        /*
        if (explosion != null)
        {
            Instantiate(explosion, transform.position, transform.rotation);
        }
        if (other.CompareTag("Player"))
        {
            Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
            gameController.GameOver();
        }
        */

        // Reacting to player's shots. Can only die to corresponding color state
        if (other.CompareTag("PlayerYellowShot") && colorState == 1)
        {
            Destroy(gameObject);
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("PlayerRedShot") && colorState == 2)
        {
            Destroy(gameObject);
            Destroy(other.gameObject);
        }
        // If player's shot does not match the enemy's color, the shot is destroyed.
        else if (other.CompareTag("PlayerYellowShot") && colorState != 1) 
        {
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("PlayerRedShot") && colorState != 2)
        {
            Destroy(other.gameObject);
        }
        
        //gameController.AddScore(scoreValue);
        //gameController.AddScore(scoreValue);
        //Destroy(other.gameObject); 
        //Destroy(gameObject); 
    }
}
