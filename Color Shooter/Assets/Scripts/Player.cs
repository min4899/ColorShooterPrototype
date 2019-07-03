using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This script defines which sprite the 'Player" uses and its health.
/// </summary>

public class Player : MonoBehaviour
{
    // Space Shooter Free script
    public GameObject destructionFX;

    public static Player instance; 

    private void Awake()
    {
        if (instance == null) 
            instance = this;
    }

    //method for damage proceccing by 'Player'
    public void GetDamage(int damage)   
    {
        Destruction();
    }    

    //'Player's' destruction procedure
    void Destruction()
    {
        Instantiate(destructionFX, transform.position, Quaternion.identity); //generating destruction visual effect and destroying the 'Player' object
        Destroy(gameObject);
    }



    private GameObject shot;
    public Transform shotSpawn;
    public float fireRate;
    public float invincibleTime;
    public float vulernableTime;
    [Tooltip("no color = 0, red = 1, yellow = 2, green = 3")]
    [Range(0, 3)]
    public int colorState;

    public Sprite red;
    public Sprite yellow;
    public Sprite green;
    public GameObject redShot;
    public GameObject yellowShot;
    public GameObject greenShot;
    public SpriteRenderer playerSprite;
    public GameObject shield;
    public Material whiteMaterial;

    private bool shieldOn;
    private float currInvincibleTime;
    private float currVulnerableTime;
    private float nextFire;
    private Material defaultMaterial;

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

    public void ChangeToRed()
    {
        //StartCoroutine(Invincible());
        //GetComponent<SpriteRenderer>().sprite = red;
        playerSprite.sprite = red;
        shot = redShot;
        colorState = 1;
        StartCoroutine(Invincible());
    }

    public void ChangeToYellow()
    {
        //StartCoroutine(Invincible());
        //GetComponent<SpriteRenderer>().sprite = yellow;
        playerSprite.sprite = yellow;
        shot = yellowShot;
        colorState = 2;
        StartCoroutine(Invincible());
    }

    public void ChangeToGreen()
    {
        //StartCoroutine(Invincible());
        //GetComponent<SpriteRenderer>().sprite = green;
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