using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using EZCameraShake;

/// <summary>
/// This script defines which sprite the 'Player" uses and its health.
/// </summary>

public class Player : MonoBehaviour
{
    public static Player instance;

    public int health = 1; //test
    public GameObject destructionFX;
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
    //public GameObject shield;
    public Animator shieldAnimator;
    public Material whiteMaterial;
    public TextMeshProUGUI healthBar;

    private GameObject shot;
    private bool shieldOn;
    private float currInvincibleTime;
    private float currVulnerableTime;
    private float nextFire;
    private Material defaultMaterial;

    private void Awake()
    {
        // just in case
        GetComponent<Player>().enabled = true;
        GetComponent<PlayerControl>().enabled = true;
        //if (instance == null) 
            instance = this;
    }

    void Start()
    {
        shieldOn = true;
        defaultMaterial = playerSprite.GetComponent<SpriteRenderer>().material;

        UpdateHealthText();

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
            AudioManager.instance.PlaySound("Player_Fire");
        }
    }

    //method for damage proceccing by 'Player'
    public void GetDamage()
    {
        health--;
        CameraShaker.Instance.ShakeOnce(2f, 2.5f, .1f, 1f);
        // update text
        UpdateHealthText();
        if (health <= 0) // dead
        {
            Destruction();
        }
        else // not dead
        {
            AudioManager.instance.PlaySound("Player_Hit");
            StartCoroutine(Invincible()); // Give Player short invincibility period 
        }
    }

    void UpdateHealthText()
    {
        string healthString = "";
        for (int i = 0; i < health; i++)
        {
            healthString += "-";
        }
        healthBar.text = healthString;
    }

    //'Player's' destruction procedure
    void Destruction()
    {
        AudioManager.instance.PlaySound("Player_Explosion");
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
        if (!shieldOn) // If player was hit while shield was down, player is damaged.
        {
            //Destruction();
            GetDamage();
        }

        // Can't change colors without shield up
        if (other.CompareTag("EnemyRedShot") && shieldOn)
        {
            ChangeToRed();
        }
        else if (other.CompareTag("EnemyYellowShot") && shieldOn)
        {
            ChangeToYellow();
        }
        else if (other.CompareTag("EnemyGreenShot") && shieldOn)
        {
            ChangeToGreen();
        }
        else if((other.CompareTag("Enemy") || other.CompareTag("Boss")) && shieldOn) // direct contact with enemy
        {
            switch (other.GetComponent<Enemy>().colorState)
            {
                case 0:
                    ChangeToNeutral();
                    break;
                case 1:
                    ChangeToRed();
                    break;
                case 2:
                    ChangeToYellow();
                    break;
                case 3:
                    ChangeToGreen();
                    break;
                default:
                    break;
            }
        }
    }

    /*
    // Short Period of Invincibility after getting hit with shield on.
    IEnumerator Invincible()
    {
        // Invincibility is on.
        GetComponent<CapsuleCollider2D>().enabled = false;
        shieldOn = false;
        shieldAnimator.SetBool("ShieldOn", false);
        //shield.SetActive(false);
        //Debug.Log("Invincibility on.");
        currInvincibleTime = invincibleTime * 10; // use 10 * seconds of invincible time to get "tenth of a second" unit.
        bool activeSprite = true;
        while (currInvincibleTime > 0)
        {
            // flicker affect
            if (activeSprite) // sprite is on
            {
                playerSprite.material = whiteMaterial;
                activeSprite = false;
            }
            else // sprite is off
            {
                playerSprite.material = defaultMaterial;
                activeSprite = true;
            }
            yield return new WaitForSeconds(0.1f); // Each loops is 0.1 seconds (tenth of a second).           
            currInvincibleTime--;
        }
        //Debug.Log("Invincibility off.");
        GetComponent<CapsuleCollider2D>().enabled = true;
        playerSprite.material = defaultMaterial;

        // Invincibility off. Must wait additional seconds before shield can get back up.
        //shieldOn = false;
        currVulnerableTime = vulernableTime; // use regular seconds
        while (currVulnerableTime > 0)
        {
            yield return new WaitForSeconds(1f); // each second
            currVulnerableTime--;
        }
        //shield.SetActive(true);
        shieldOn = true;
        shieldAnimator.SetBool("ShieldOn", true);
        AudioManager.instance.PlaySound("Player_Shield_Regen");
    }
    */

    // Function to handle shield break and regen
    IEnumerator ShieldHit()
    {
        StartCoroutine(Invincible()); // test
        AudioManager.instance.PlaySound("Player_Shield_Hit");
        shieldOn = false;
        shieldAnimator.SetBool("ShieldOn", false);
        currVulnerableTime = vulernableTime; // use regular seconds
        while (currVulnerableTime > 0)
        {
            yield return new WaitForSeconds(1f); // each second
            currVulnerableTime--;
        }
        //shield.SetActive(true);
        shieldOn = true;
        shieldAnimator.SetBool("ShieldOn", true);
        AudioManager.instance.PlaySound("Player_Shield_Regen");
    }

    // Short Period of Invincibility after getting hit/damaged
    IEnumerator Invincible()
    {
        // Invincibility is on.
        GetComponent<CapsuleCollider2D>().enabled = false;
        currInvincibleTime = invincibleTime * 10; // use 10 * seconds of invincible time to get "tenth of a second" unit.
        bool activeSprite = true;
        while (currInvincibleTime > 0)
        {
            // flicker affect
            if (activeSprite) // sprite is on
            {
                playerSprite.material = whiteMaterial;
                activeSprite = false;
            }
            else // sprite is off
            {
                playerSprite.material = defaultMaterial;
                activeSprite = true;
            }
            yield return new WaitForSeconds(0.1f); // Each loops is 0.1 seconds (tenth of a second).           
            currInvincibleTime--;
        }
        //Debug.Log("Invincibility off.");
        playerSprite.material = defaultMaterial;
        GetComponent<CapsuleCollider2D>().enabled = true;
    }

    // Following 4 methods processes the taking damage, effect, and changing states.
    public void ChangeToNeutral()
    {
        playerSprite.sprite = defaultSprite;
        shot = null;
        colorState = 0;
        StartCoroutine(ShieldHit());
        //StartCoroutine(Invincible());
    }

    public void ChangeToRed()
    {
        playerSprite.sprite = red;
        shot = redShot;
        colorState = 1;
        StartCoroutine(ShieldHit());
        //StartCoroutine(Invincible());
    }

    public void ChangeToYellow()
    {
        playerSprite.sprite = yellow;
        shot = yellowShot;
        colorState = 2;
        StartCoroutine(ShieldHit());
        //StartCoroutine(Invincible());
    }

    public void ChangeToGreen()
    {
        playerSprite.sprite = green;
        shot = greenShot;
        colorState = 3;
        StartCoroutine(ShieldHit());
        //StartCoroutine(Invincible());
    }

    public bool CheckShield()
    {
        return shieldOn;
    }
}