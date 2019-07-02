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

    [Tooltip("no color = 0, red = 1, yellow = 2, green = 3")]
    [Range(0, 3)]
    public int colorState;

    [Tooltip("min should be 1, how many hits it can take")]
    public int health;

    [Tooltip("Spawn another projectile or enemy on death via SpawnOnDeath Script")]
    public bool spawnOnDeath = false;

    public Material whiteMaterial;
    public SpriteRenderer enemySprite;

    private GameObject player;
    private PlayerControl playercontrol;
    private Material defaultMaterial;

    void Start()
    {
        defaultMaterial = enemySprite.GetComponent<SpriteRenderer>().material;
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
        if (other.CompareTag("Boundary") || other.CompareTag("Enemy") || other.CompareTag("EnemyRedShot") || 
            other.CompareTag("EnemyYellowShot") || other.CompareTag("EnemyGreenShot") || other.CompareTag("Boss"))
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
        if (other.CompareTag("PlayerRedShot") && colorState == 1)
        {
            //Destroy(gameObject);
            StartCoroutine(TakeDamage());
            health--;
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("PlayerYellowShot") && colorState == 2)
        {
            //Destroy(gameObject);
            StartCoroutine(TakeDamage());
            health--;
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("PlayerGreenShot") && colorState == 3)
        {
            //Destroy(gameObject);
            StartCoroutine(TakeDamage());
            health--;
            Destroy(other.gameObject);
        }
        // If player's shot does not match the enemy's color, no damage is taken and the shot is destroyed.
        else if (other.CompareTag("PlayerRedShot") && colorState != 1) 
        {
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("PlayerYellowShot") && colorState != 2)
        {
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("PlayerGreenShot") && colorState != 3)
        {
            Destroy(other.gameObject);
        }

        // if health is 0, destroy object
        if (health <= 0)
        {
            if(spawnOnDeath)
            {
                gameObject.GetComponent<SpawnOnDeath>().Spawn();
            }
            Destroy(gameObject);
        }

        //gameController.AddScore(scoreValue);
        //gameController.AddScore(scoreValue);
        //Destroy(other.gameObject); 
        //Destroy(gameObject); 
    }

    IEnumerator TakeDamage()
    {
        if(defaultMaterial != null && whiteMaterial != null)
        {
            //gameObject.GetComponent<SpriteRenderer>().material = whiteMaterial;
            enemySprite.material = whiteMaterial;
            yield return new WaitForSeconds(0.05f);
            //gameObject.GetComponent<SpriteRenderer>().material = defaultMaterial;
            enemySprite.material = defaultMaterial;
        }
    }
}
