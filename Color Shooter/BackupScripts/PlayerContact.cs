using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Contact with player
public class PlayerContact : MonoBehaviour {

    public int colorState;

    private PlayerControl playerControl;

    // Use this for initialization
    void Start () {
        playerControl = gameObject.GetComponent<PlayerControl>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Boundary") || other.CompareTag("PlayerRedShot") || other.CompareTag("PlayerYellowShot"))
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

        if (other.CompareTag("EnemyRedShot") && playerControl.CheckShield())
        {
            playerControl.ChangeToRed();
        }
        else if (other.CompareTag("EnemyYellowShot") && playerControl.CheckShield())
        {
            playerControl.ChangeToYellow();
        }
        else
        {
            Destroy(gameObject);
        }
        Destroy(other.gameObject);
        //gameController.AddScore(scoreValue);
        //Destroy(gameObject);
    }
}
