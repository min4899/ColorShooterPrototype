using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WashingMachine : MonoBehaviour {

    #region FIELDS
    [Tooltip("Health points in integer")]
    public int health = 1;

    public int points;
    
    [Tooltip("no color = 0, red = 1, yellow = 2, green = 3")]
    [Range(0, 3)]
    public int colorState;

    public GameObject shotPhase1;
    public GameObject shotPhase2;
    public float fireRate = 0.7f;
    
    //[Tooltip("Enemy's projectile prefab")]
    //public GameObject Projectile;

    [Tooltip("VFX prefab generating after destruction")]
    public GameObject destructionVFX;
    //public string destructionSound;
    public GameObject hitEffect;
    //public string fireSound;
    [Tooltip("Score points to display if enemy is destroyed.")]
    public GameObject pointText;
    public Material whiteMaterial;
    public SpriteRenderer enemySprite;

    private Material defaultMaterial;
    private GameObject currentShotPhase;
    private int fullHP;

    [HideInInspector]public bool damaged = false;
    #endregion

    void Start()
    {
        if (GameController.instance != null)
        {
            GameController.instance.enemyCount++;
        }
        //GameController.instance.enemyCount++;
        if (enemySprite != null)
        {
            defaultMaterial = enemySprite.GetComponent<SpriteRenderer>().material;
        }
        fullHP = health;
        damaged = false;
        GetComponent<Animator>().SetBool("damaged", false);
        currentShotPhase = Instantiate(shotPhase1, gameObject.transform);
    }

    void Update()
    {
        if (health >= fullHP * 2 / 3) // over 2/3
        {
            currentShotPhase.GetComponent<EnemyShooting2>().fireRate = fireRate;
            currentShotPhase.GetComponent<EnemyShooting2>().sequenceDelay = fireRate;
        }
        else if (health >= fullHP / 3 && health < fullHP * 2 / 3) // between 1/3 and 2/3
        {
            currentShotPhase.GetComponent<EnemyShooting2>().fireRate = fireRate - 0.1f;
            currentShotPhase.GetComponent<EnemyShooting2>().sequenceDelay = fireRate - 0.1f;
        }
        else if (health < fullHP / 3) // less than 1/3
        {
            currentShotPhase.GetComponent<EnemyShooting2>().fireRate = fireRate - 0.2f;
            currentShotPhase.GetComponent<EnemyShooting2>().sequenceDelay = fireRate - 0.2f;
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
            if (hitEffect != null)
            {
                Instantiate(hitEffect, transform.position, Quaternion.identity, transform);
            }
            StartCoroutine(TakeDamageEffect());
        }
    }

    public void Damaged()
    {
        damaged = true;
        GetComponent<Animator>().SetBool("damaged", true);
        Destroy(currentShotPhase.transform.gameObject);
        currentShotPhase = Instantiate(shotPhase2, gameObject.transform); ;
    }

    public void NotDamaged()
    {
        damaged = false;
        GetComponent<Animator>().SetBool("damaged", false);
        Destroy(currentShotPhase.transform.gameObject);
        currentShotPhase = Instantiate(shotPhase1, gameObject.transform);
    }

    /*
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Boundary") || other.CompareTag("Enemy") || other.CompareTag("EnemyRedShot") ||
            other.CompareTag("EnemyYellowShot") || other.CompareTag("EnemyGreenShot") || other.CompareTag("Boss"))
        {
            return;
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
    */

    void OnTriggerEnter2D(Collider2D other)
    {
        if (damaged)
        {
            if (other.CompareTag("Boundary") || other.CompareTag("Enemy") || other.CompareTag("EnemyRedShot") ||
                other.CompareTag("EnemyYellowShot") || other.CompareTag("EnemyGreenShot") || other.CompareTag("Boss"))
            {
                return;
            }
            // Reacting to player's shots.
            else if (other.CompareTag("PlayerRedShot") || other.CompareTag("PlayerYellowShot") || other.CompareTag("PlayerGreenShot"))
            {
                GetDamage(Player.instance.shotDamage);
            }
        }
        if (other.CompareTag("Player"))
        {
            if(Player.instance.CheckShield()) // shield on
            {
                Player.instance.ChangeToNeutral();
            }
            else // shield off
            {
                Player.instance.GetDamage();
            }
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
        if (destructionVFX != null)
        {
            //AudioManager.instance.PlaySound(destructionSound);
            AudioManager.instance.PlaySound("Enemy_Explosion");
            Instantiate(destructionVFX, transform.position, Quaternion.identity);
        }
        if (GameController.instance != null)
            GameController.instance.EndLevel();

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
            GameController.instance.colorCombo++;
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
