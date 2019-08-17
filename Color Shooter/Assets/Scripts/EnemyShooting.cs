using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour {

    [Tooltip("Object to be used as shots, use parent of multiple shots to shoot different types of shots.")]
    public GameObject shot;
    public string fireSound = "Enemy_Fire";
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

    /*
    [Tooltip("Projectiles to be shot out upon death, see Enemy script for activation.")]
    public Transform[] deathSpawn;
    public GameObject deathShot;
    */

    private Transform player; // used for player aiming
    private GameObject currentShot;
    private int shotIndex;
    private int multipleShotsBetweenCounter;
    //private string fireSound;

    // Use this for initialization
    void Start()
    {
        /*
        if (gameObject.GetComponent<Enemy>() != null)
        {
            fireSound = gameObject.GetComponent<Enemy>().fireSound;
        }
        */
        if (multipleShots) // if using multiple shot types
        {
            shotIndex = 0;
            multipleShotsBetweenCounter = multipleShotsBetween;
            currentShot = shot.transform.GetChild(shotIndex).gameObject;
        }
        else // for only 1 shot type
        {
            currentShot = shot;
        }
        /*
        if (aim)
        {
            GameObject playerObject = GameObject.FindWithTag("Player");
            if (playerObject != null)
            {
                player = playerObject.GetComponent<Transform>();
            }
        }
        */
        if(burstFire)
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
        if(aim && Player.instance != null) // constantly update reference to player's location
        {
            player = Player.instance.GetComponent<Transform>();
            aimTransform.up = player.position - aimTransform.position;
        }
        /*
        if (aim && player != null)
        {
            aimTransform.up = player.position - aimTransform.position;
        }
        */
    }

    void Fire()
    {
        //Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
        //GetComponent<AudioSource>().Play();
        //gameObject.GetComponent<Animator>().SetTrigger("Fire");

        for(int i = 0; i < shotSpawn.Length; i++)
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
        while(gameObject != null)
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
    
    /*
    public void FireUponDeath()
    {
        //Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
        //GetComponent<AudioSource>().Play();
        for (int i = 0; i < deathSpawn.Length; i++)
        {
            Instantiate(shot, deathSpawn[i].position, deathSpawn[i].rotation);
        }
    }
    */

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
        }
    }
}
