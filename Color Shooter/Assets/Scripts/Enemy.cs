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

    public int points;

    [Tooltip("no color = 0, red = 1, yellow = 2, green = 3")]
    [Range(0, 3)]
    public int colorState;

    //[Tooltip("Enemy's projectile prefab")]
    //public GameObject Projectile;

    [Tooltip("VFX prefab generating after destruction")]
    public GameObject destructionVFX;
    //public string destructionSound;
    public GameObject hitEffect;
    //public string fireSound;
    [Tooltip("Score points to display if enemy is destroyed.")]
    public GameObject pointText;

    //[HideInInspector] public int shotChance; //probability of 'Enemy's' shooting during tha path
    //[HideInInspector] public float shotTimeMin, shotTimeMax; //max and min time for shooting from the beginning of the path

    //[Tooltip("If enemy is the final boss of the level, level will end upon its death. Set tag as 'Boss'")]
    //public bool boss = false;
    [Tooltip("Spawn another enemy on death via SpawnOnDeath Script")]
    public bool spawnOnDeath = false;
    [Tooltip("Spawn projectiles on death")]
    public bool fireOnDeath = false;
    [Tooltip("Enemy dies immediately if it touches the player")]
    public bool DeathOnContact = false;

    public Material whiteMaterial;
    public SpriteRenderer enemySprite;

    private Material defaultMaterial;
    //private GameObject gameController;
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
        if(GameController.instance != null)
        {
            GameController.instance.enemyCount++;
        }
        //GameController.instance.enemyCount++;
        if (enemySprite != null)
        {
            defaultMaterial = enemySprite.GetComponent<SpriteRenderer>().material;
        }
    }

    //method of getting damage for the 'Enemy'
    public void GetDamage(int damage) 
    {
        AudioManager.instance.PlaySound("Enemy_Hit");
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
            /*
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
            */
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
    public void Destruction()                           
    {        
        if(destructionVFX != null)
        {
            //AudioManager.instance.PlaySound(destructionSound);
            AudioManager.instance.PlaySound("Enemy_Explosion");
            Instantiate(destructionVFX, transform.position, Quaternion.identity);
        }
        if (spawnOnDeath)
        {
            GetComponent<SpawnOnDeath>().Spawn();
        }
        if(fireOnDeath)
        {
            //GetComponent<EnemyShooting>().FireUponDeath();
            GetComponent<FireOnDeath>().FireUponDeath();
        }
        //if (boss)
        if(gameObject.CompareTag("Boss"))
        {
            if (GameObject.FindGameObjectsWithTag("Boss").Length <= 1) // If this was the only remaining boss.
            {
                //boss = false;
                if (GameController.instance != null)
                    GameController.instance.EndLevel();
            }
        }
        SetScore();
        Destroy(gameObject);
    }

    void SetScore()
    {
        // Set overall score earned for killing this enemy
        int total = 0;
        if (!Player.instance.CheckShield()) // If player kills an enemy while shield is down, give me more points
        {
            total += 20;
        }

        // increment enemy killed count and overall score in gamecontroller
        if (GameController.instance != null)
        {
            if (colorState == GameController.instance.lastColorDestroyed) // killing same color enemies gives combo
            {
                GameController.instance.colorCombo++;
                //total += points + points * GameController.instance.colorCombo;
            }
            else // not the same color, reset counter
            {
                GameController.instance.lastColorDestroyed = colorState;
                GameController.instance.colorCombo = 1;
                //total += points;
            }
            total += points * GameController.instance.colorCombo;
            GameController.instance.enemiesKilled++;
            GameController.instance.score += total;
            GameController.instance.UpdateScore();
            GameController.instance.UpdateMultiplier();
        }
        else // gamecontroller not on (for testing in editor), just give standard amount of points
        {
            total += points;
        }

        // Activate this enemy's score animation
        if (pointText != null)
        {
            Vector2 screenPosition = Camera.main.WorldToScreenPoint(gameObject.transform.position);
            GameObject canvas = GameObject.Find("Canvas");
            GameObject newScoreText = Instantiate(pointText, screenPosition, Quaternion.identity, canvas.transform);
            //newScoreText.transform.position = screenPosition;
            newScoreText.GetComponent<ScoreTextController>().SetText(total.ToString());
            //newScoreText.GetComponent<TextMeshProUGUI>().SetText("+" + total);
        }
    }
}
