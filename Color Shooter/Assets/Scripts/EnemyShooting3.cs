using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Enemy changes color based on what color shot it shoots
public class EnemyShooting3 : MonoBehaviour {
    public SpriteRenderer enemySprite;
    public Sprite green;
    public Sprite yellow;
    public Sprite red;
    public float colorChangeDelay;

    [Tooltip("Object to be used as shots, use parent of multiple shots to shoot different types of shots.")]
    public GameObject shot;
    [Tooltip("Select to shoot multiple types of shots. Use parent of multiple types of shots in 'shot' param")]
    public bool multipleShots = false;
    [Tooltip("How many shots is to be fired before switching to the next shot type")]
    public int multipleShotsBetween = 1;
    [Tooltip("Enemy will aim its shot at player's location.")]
    public bool aim;
    [Tooltip("The shotSpawn/parent of shotSpawns that represents the general direction.")]
    public Transform aimTransform;
    public Transform[] shotSpawn;

    //public Transform shotSpawn;
    [Tooltip("Time that enemy won't fire between shots/burst shots")]
    public float fireRate;
    [Tooltip("Start shooting only after a delay time")]
    public float delay;

    public bool burstFire;
    public int burstSize;
    public float burstDelay;

    [Tooltip("Projectiles to be shot out upon death, see Enemy script for activation.")]
    public Transform[] deathSpawn;
    public GameObject deathShot;

    private Transform player; // used for player aiming
    private Enemy enemyScript;
    private GameObject currentShot;
    private int shotIndex;
    private int multipleShotsBetweenCounter;
    private string fireSound;

    // Use this for initialization
    void Start()
    {
        enemyScript = gameObject.GetComponent<Enemy>();
        if(enemyScript != null)
        {
            fireSound = enemyScript.fireSound;
        }
        if (multipleShots) // if using multiple shot types
        {
            shotIndex = 0;
            multipleShotsBetweenCounter = multipleShotsBetween;
            currentShot = shot.transform.GetChild(shotIndex).gameObject;
            enemyScript.colorState = GetState(currentShot.tag);
            enemySprite.sprite = GetColor(currentShot.tag);
        }
        else // for only 1 shot type
        {
            currentShot = shot;
        }
        if (aim)
        {
            GameObject playerObject = GameObject.FindWithTag("Player");
            if (playerObject != null)
            {
                player = playerObject.GetComponent<Transform>();
            }
        }
        if (burstFire)
        {
            StartCoroutine(BurstFire());
        }
        else
        {
            InvokeRepeating("Fire", delay, fireRate);
        }
        //InvokeRepeating("Fire", delay, fireRate);
    }

    // Update is called once per frame
    void Update()
    {
        if (aim && player != null)
        {
            aimTransform.up = player.position - aimTransform.position;
        }
    }

    void Fire()
    {
        //Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
        //GetComponent<AudioSource>().Play();
        for (int i = 0; i < shotSpawn.Length; i++)
        {
            AudioManager.instance.PlaySound(fireSound);
            Instantiate(currentShot, shotSpawn[i].position, shotSpawn[i].rotation);
        }
        if (multipleShots)
        {
            NextShot();
        }
    }

    IEnumerator BurstFire()
    {
        yield return new WaitForSeconds(delay);
        while (gameObject != null)
        {
            for (int i = 0; i < burstSize; i++)
            {
                Fire();
                if (i < burstSize - 1) // dont add delay for last burst
                {
                    yield return new WaitForSeconds(burstDelay);
                }
            }
            yield return new WaitForSeconds(fireRate);
        }
    }

    void NextShot()
    {
        multipleShotsBetweenCounter--;
        if (multipleShotsBetweenCounter <= 0)
        {
            multipleShotsBetweenCounter = multipleShotsBetween;
            shotIndex++;
            if (shotIndex >= shot.transform.childCount)
            {
                shotIndex = 0;
            }
            currentShot = shot.transform.GetChild(shotIndex).gameObject;
            //StartCoroutine(SwitchColor(currentShot.GetComponent<Enemy>().colorState));
            StartCoroutine(SwitchColor(currentShot.tag));
        }
    }

    IEnumerator SwitchColor(string state)
    {
        yield return new WaitForSeconds(colorChangeDelay);
        enemyScript.colorState = GetState(state);
        enemySprite.sprite = GetColor(state);
    }

    Sprite GetColor(string state)
    {
        switch (state)
        {
            case "EnemyRedShot":
                return red;
            case "EnemyYellowShot":
                return yellow;
            case "EnemyGreenShot":
                return green;
            default:
                return enemySprite.sprite;
        }
    }

    int GetState(string state)
    {
        switch (state)
        {
            case "EnemyRedShot":
                return 1;
            case "EnemyYellowShot":
                return 2;
            case "EnemyGreenShot":
                return 3;
            default:
                return enemyScript.colorState;
        }
    }
}
