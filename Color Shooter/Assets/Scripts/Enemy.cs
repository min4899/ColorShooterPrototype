using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script defines 'Enemy's' health and behavior. 
/// </summary>
public class Enemy : MonoBehaviour {

    #region FIELDS
    [Tooltip("Health points in integer")]
    public int health = 1;

    [Tooltip("no color = 0, red = 1, yellow = 2, green = 3")]
    [Range(0, 3)]
    public int colorState;

    //[Tooltip("Enemy's projectile prefab")]
    //public GameObject Projectile;

    [Tooltip("VFX prefab generating after destruction")]
    public GameObject destructionVFX;
    public GameObject hitEffect;

    //[HideInInspector] public int shotChance; //probability of 'Enemy's' shooting during tha path
    //[HideInInspector] public float shotTimeMin, shotTimeMax; //max and min time for shooting from the beginning of the path

    [Tooltip("Spawn another enemy on death via SpawnOnDeath Script")]
    public bool spawnOnDeath = false;
    [Tooltip("Spawn projectiles on death")]
    public bool fireOnDeath = false;
    [Tooltip("Enemy dies immediately if it touches the player")]
    public bool DeathOnContact = false;

    public Material whiteMaterial;
    public SpriteRenderer enemySprite;
    private Material defaultMaterial;
    #endregion

    // Provided Shot Generator
    /*
    private void Start()
    {
        Invoke("ActivateShooting", Random.Range(shotTimeMin, shotTimeMax));
    }

    //coroutine making a shot
    void ActivateShooting() 
    {
        if (Random.value < (float)shotChance / 100)                             //if random value less than shot probability, making a shot
        {                         
            Instantiate(Projectile,  gameObject.transform.position, Quaternion.identity);             
        }
    }
    */

    void Start()
    {
        if (enemySprite != null)
        {
            defaultMaterial = enemySprite.GetComponent<SpriteRenderer>().material;
        }
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

    //method of getting damage for the 'Enemy'
    public void GetDamage(int damage) 
    {
        health -= damage;           //reducing health for damage value, if health is less than 0, starting destruction procedure
        if (health <= 0)
        {
            Destruction();
        }
        else
        {
            if(hitEffect != null)
            {
                Instantiate(hitEffect, transform.position, Quaternion.identity, transform);
            }
            StartCoroutine(TakeDamageEffect());
        }
    }

    /*
    //if 'Enemy' collides 'Player', 'Player' gets the damage equal to projectile's damage value
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (Projectile.GetComponent<Projectile>() != null)
                Player.instance.GetDamage(Projectile.GetComponent<Projectile>().damage);
            else
                Player.instance.GetDamage(1);
        }
    }
    */

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Boundary") || other.CompareTag("Enemy") || other.CompareTag("EnemyRedShot") ||
            other.CompareTag("EnemyYellowShot") || other.CompareTag("EnemyGreenShot") || other.CompareTag("Boss"))
        {
            return;
        }
        // Contact with Player
        if(other.CompareTag("Player"))
        {
            // Effect on Player
            switch (colorState)
            {
                case 0:
                    Player.instance.ChangeToNeutral();
                    break;
                case 1:
                    Player.instance.ChangeToRed();
                    break;
                case 2:
                    Player.instance.ChangeToYellow();
                    break;
                case 3:
                    Player.instance.ChangeToGreen();
                    break;
                default:
                    break;
            }
            // Effect on enemy itself
            if (DeathOnContact)
            {
                Destruction();
            }
            else
            {
                GetDamage(Player.instance.contactDamage);
            }
        }
        // Reacting to player's shots. Can only die to corresponding color state
        else if (other.CompareTag("PlayerRedShot") && colorState == 1)
        {
            GetDamage(Player.instance.shotDamage);
            //Destroy(other.gameObject);
        }
        else if (other.CompareTag("PlayerYellowShot") && colorState == 2)
        {
            GetDamage(Player.instance.shotDamage);
            //Destroy(other.gameObject);
        }
        else if (other.CompareTag("PlayerGreenShot") && colorState == 3)
        {
            GetDamage(Player.instance.shotDamage);
            //Destroy(other.gameObject);
        }
        /*
        // If player's shot does not match the enemy's color, no damage is taken and the player shot is destroyed.
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
        */
        //gameController.AddScore(scoreValue);
        //gameController.AddScore(scoreValue);
        //Destroy(other.gameObject); 
        //Destroy(gameObject); 
    }

    IEnumerator TakeDamageEffect()
    {
        if (defaultMaterial != null && whiteMaterial != null)
        {
            //gameObject.GetComponent<SpriteRenderer>().material = whiteMaterial;
            enemySprite.material = whiteMaterial;
            yield return new WaitForSeconds(0.05f);
            //gameObject.GetComponent<SpriteRenderer>().material = defaultMaterial;
            enemySprite.material = defaultMaterial;
        }
    }

    //method of destroying the 'Enemy'
    void Destruction()                           
    {        
        //Instantiate(destructionVFX, transform.position, Quaternion.identity);
        if(destructionVFX != null)
        {
            Instantiate(destructionVFX, transform.position, Quaternion.identity);
        }
        if (spawnOnDeath)
        {
            GetComponent<SpawnOnDeath>().Spawn();
        }
        if(fireOnDeath)
        {
            GetComponent<EnemyShooting>().FireUponDeath();
        }
        Destroy(gameObject);
    }
}
