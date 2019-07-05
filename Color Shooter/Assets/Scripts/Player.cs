using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This script defines which sprite the 'Player" uses and its health.
/// </summary>

public class Player : MonoBehaviour
{
    public GameObject destructionFX;
    public static Player instance;

    public Transform shotSpawn;
    public int shotDamage = 1;
    public int contactDamage = 1;
    public float fireRate;
    public float invincibleTime;
    public float vulernableTime;
    [Tooltip("no color = 0, red = 1, yellow = 2, green = 3")]
    [Range(0, 3)]
    public int colorState;

    public Sprite defaultSprite;
    public Sprite red;
    public Sprite yellow;
    public Sprite green;
    public GameObject redShot;
    public GameObject yellowShot;
    public GameObject greenShot;
    public SpriteRenderer playerSprite;
    public GameObject shield;
    public Material whiteMaterial;

    private GameObject shot;
    private bool shieldOn;
    private float currInvincibleTime;
    private float currVulnerableTime;
    private float nextFire;
    private Material defaultMaterial;

    private void Awake()
    {
        if (instance == null) 
            instance = this;
    }

    void Start()
    {
        shieldOn = true;
        defaultMaterial = playerSprite.GetComponent<SpriteRenderer>().material;

        // for testing purposes
        if (colorState == 1)
        {
            playerSprite.sprite = red;
            shot = redShot;
        }
        else if (colorState == 2)
        {
            playerSprite.sprite = yellow;
            shot = yellowShot;
        }
        else if (colorState == 3)
        {
            playerSprite.sprite = green;
            shot = greenShot;
        }
    }

    void Update()
    {
        /*
        if (shot != null && Input.GetButton("Fire1") && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
            //GetComponent<AudioSource>().Play();
        }
        */
        //constantly fire
        if (shot != null && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
            //GetComponent<AudioSource>().Play();
        }
    }

    //method for damage proceccing by 'Player'
    public void GetDamage(int damage)
    {
        Destruction();
    }

    //'Player's' destruction procedure
    void Destruction()
    {
        if (destructionFX != null)
        {
            Instantiate(destructionFX, transform.position, Quaternion.identity); //generating destruction visual effect
        }
        //Instantiate(destructionFX, transform.position, Quaternion.identity); //generating destruction visual effect
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Boundary") || other.CompareTag("PlayerRedShot") || other.CompareTag("PlayerYellowShot") || other.CompareTag("PlayerGreenShot"))
        {
            return;
        }
        // Only enemy tag and the 3 enemy shots tags should be left.
        if (!shieldOn) // If player was hit while shield was down, player is killed.
        {
            Destruction();
        }
        // NOTE: Contact with an Enemy, see Enemy Script for changing player's state on enemy contact (Line 121).
        
        else if (other.CompareTag("EnemyRedShot") && shieldOn)
        {
            ChangeToRed();
            //Destroy(other.gameObject);
        }
        else if (other.CompareTag("EnemyYellowShot") && shieldOn)
        {
            ChangeToYellow();
            //Destroy(other.gameObject);
        }
        else if (other.CompareTag("EnemyGreenShot") && shieldOn)
        {
            ChangeToGreen();
            //Destroy(other.gameObject);
        }
        //gameController.AddScore(scoreValue);
    }

    // Short Period of Invincibility after getting hit with shield on.
    IEnumerator Invincible()
    {
        // Invincibility is on.
        GetComponent<CapsuleCollider2D>().enabled = false;
        //shieldOn = false;
        shield.SetActive(false);
        //Debug.Log("Invincibility on.");
        currInvincibleTime = invincibleTime * 10; // use 10 * seconds of invincible time to get "tenth of a second" unit.
        bool activeSprite = true;
        while (currInvincibleTime > 0)
        {
            // flicker affect
            if (activeSprite) // sprite is on
            {
                //gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.3f);
                //gameObject.GetComponent<SpriteRenderer>().material = whiteMaterial;
                playerSprite.material = whiteMaterial;
                activeSprite = false;
            }
            else // sprite is off
            {
                //gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                //gameObject.GetComponent<SpriteRenderer>().material = defaultMaterial;
                playerSprite.material = defaultMaterial;
                activeSprite = true;
            }
            yield return new WaitForSeconds(0.1f); // Each loops is 0.1 seconds (tenth of a second).           
            currInvincibleTime--;
        }
        //Debug.Log("Invincibility off.");
        GetComponent<CapsuleCollider2D>().enabled = true;
        //gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        //gameObject.GetComponent<SpriteRenderer>().material = defaultMaterial;
        playerSprite.material = defaultMaterial;

        // Invincibility off. Must wait additional seconds before shield can get back up.
        shieldOn = false;
        currVulnerableTime = vulernableTime; // use regular seconds
        while (currVulnerableTime > 0)
        {
            yield return new WaitForSeconds(1f); // each second
            currVulnerableTime--;
        }
        shield.SetActive(true);
        shieldOn = true;
    }

    // Following 4 methods processes the taking damage, effect, and changing states.
    public void ChangeToNeutral()
    {
        playerSprite.sprite = defaultSprite;
        shot = null;
        colorState = 0;
        StartCoroutine(Invincible());
    }

    public void ChangeToRed()
    {
        playerSprite.sprite = red;
        shot = redShot;
        colorState = 1;
        StartCoroutine(Invincible());
    }

    public void ChangeToYellow()
    {
        playerSprite.sprite = yellow;
        shot = yellowShot;
        colorState = 2;
        StartCoroutine(Invincible());
    }

    public void ChangeToGreen()
    {
        playerSprite.sprite = green;
        shot = greenShot;
        colorState = 3;
        StartCoroutine(Invincible());
    }

    public bool CheckShield()
    {
        return shieldOn;
    }
}